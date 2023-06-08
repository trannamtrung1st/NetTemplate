using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetTemplate.ApacheKafka.Utils;
using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Extensions;
using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Interfaces;
using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Models;

namespace NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Implementations
{
    public abstract class CompetingThreadConsumer<TConsumer, TKey, TValue> : IGeneralConsumer
    {
        private IConfigurationSection _consumerConfigurationSection;

        protected readonly IServiceProvider provider;
        protected readonly IExternalOffsetStore externalOffsetStore;
        protected readonly IConfiguration configuration;
        protected readonly ILogger<TConsumer> logger;
        protected readonly IOptions<ApacheKafkaConfig> kafkaOptions;
        protected readonly SemaphoreSlim offsetStoreLock;
        protected bool offsetStoreInit;

        public CompetingThreadConsumer(
            IServiceProvider provider,
            IExternalOffsetStore externalOffsetStore,
            IConfiguration configuration,
            IOptions<ApacheKafkaConfig> kafkaOptions,
            ILogger<TConsumer> logger)
        {
            this.provider = provider;
            this.externalOffsetStore = externalOffsetStore;
            this.configuration = configuration;
            this.kafkaOptions = kafkaOptions;
            this.logger = logger;

            offsetStoreLock = new SemaphoreSlim(1);
            offsetStoreInit = false;
        }

        private bool _enabled;
        public virtual bool Enabled
        {
            get
            {
                if (_consumerConfigurationSection == null)
                {
                    _consumerConfigurationSection = configuration.GetConsumerConfig(ConsumerName);
                    _enabled = _consumerConfigurationSection.Exists();
                }

                return _enabled;
            }
        }

        public virtual Task Start(CancellationToken cancellationToken = default)
        {
            CompetingConsumerConfig consumerConfig = GetConfig();

            if (consumerConfig != null)
            {
                for (int i = 1; i <= consumerConfig.ConsumerCount; i++)
                {
                    StartThread(i, consumerConfig, cancellationToken);
                }
            }

            return Task.CompletedTask;
        }

        protected virtual void StartThread(int threadId, CompetingConsumerConfig consumerConfig, CancellationToken cancellationToken = default)
        {
            Thread thread = new Thread(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        await Consume(threadId, consumerConfig, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, ex.Message);

                        await Task.Delay(consumerConfig.DefaultRetryAfter);
                    }
                }
            })
            {
                IsBackground = true
            };

            thread.Start();
        }

        protected virtual async Task Consume(int threadId, CompetingConsumerConfig consumerConfig, CancellationToken cancellationToken = default)
        {
            consumerConfig = (CompetingConsumerConfig)consumerConfig.Clone();
            consumerConfig.GroupInstanceId = threadId.ToString();

            using IConsumer<TKey, TValue> consumer = KafkaHelper.CreateConsumer<TKey, TValue>(consumerConfig);

            try
            {
                consumer.Subscribe(Topics);

                if (consumerConfig.UseExternalOffsetStore)
                {
                    try
                    {
                        offsetStoreLock.Wait(cancellationToken);

                        if (!offsetStoreInit)
                        {
                            IEnumerable<TopicPartitionOffset> offsets = await externalOffsetStore.GetStoredOffsets(Topics, consumerConfig.GroupId);

                            consumer.Commit(offsets);

                            offsetStoreInit = true;
                        }
                    }
                    finally
                    {
                        offsetStoreLock.Release();
                    }
                }

                while (!cancellationToken.IsCancellationRequested)
                {
                    ConsumeResult<TKey, TValue> message = consumer.Consume(cancellationToken);

                    using (IServiceScope scope = provider.CreateScope())
                    {
                        await Handle(message.Topic, message.Message.Key, message.Message.Value, scope.ServiceProvider, cancellationToken);
                    }

                    if (consumerConfig.EnableAutoCommit != true)
                    {
                        try
                        {
                            consumer.Commit();

                            if (consumerConfig.UseExternalOffsetStore)
                            {
                                await externalOffsetStore.StoreOffsets(new[] { message.TopicPartitionOffset }, consumerConfig.GroupId);
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, ex.Message);
                        }
                    }
                }
            }
            finally
            {
                consumer.Close();
            }
        }

        protected abstract string[] Topics { get; }
        protected abstract string ConsumerName { get; }

        protected virtual CompetingConsumerConfig GetConfig()
        {
            if (!Enabled) throw new InvalidOperationException();

            CompetingConsumerConfig consumerConfig = (CompetingConsumerConfig)kafkaOptions.Value.CommonConsumerConfig.Clone();

            _consumerConfigurationSection.Bind(consumerConfig);

            return consumerConfig;
        }

        protected abstract Task Handle(string topic, TKey key, TValue value, IServiceProvider provider, CancellationToken cancellationToken = default);
    }
}
