using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetTemplate.ApacheKafka.Utils;
using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Interfaces;
using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Models;

namespace NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Implementations
{
    public abstract class CompetingThreadConsumer<TConsumer, TKey, TValue> : IGeneralConsumer
    {
        protected readonly IServiceProvider provider;
        protected readonly IOffsetStore offsetStore;
        protected readonly IConfiguration configuration;
        protected readonly ILogger<TConsumer> logger;
        protected readonly SemaphoreSlim offsetLock;
        protected bool offsetInit;

        public CompetingThreadConsumer(
            IServiceProvider provider,
            IOffsetStore offsetStore,
            IConfiguration configuration,
            ILogger<TConsumer> logger)
        {
            this.provider = provider;
            this.offsetStore = offsetStore;
            this.configuration = configuration;
            this.logger = logger;

            offsetLock = new SemaphoreSlim(1);
            offsetInit = false;
        }

        public Task Start(CompetingConsumerConfig commonConfig, CancellationToken cancellationToken = default)
        {
            CompetingConsumerConfig consumerConfig = GetConfig(commonConfig);

            for (int i = 1; i <= consumerConfig.ConsumerCount; i++)
            {
                StartThread(i, consumerConfig, cancellationToken);
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

                if (consumerConfig.UseOffsetStore)
                {
                    try
                    {
                        offsetLock.Wait(cancellationToken);

                        if (!offsetInit)
                        {
                            IEnumerable<TopicPartitionOffset> offsets = await offsetStore.GetStoredOffsets(Topics, consumerConfig.GroupId);

                            consumer.Commit(offsets);

                            offsetInit = true;
                        }
                    }
                    finally
                    {
                        offsetLock.Release();
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

                            if (consumerConfig.UseOffsetStore)
                            {
                                await offsetStore.StoreOffsets(new[] { message.TopicPartitionOffset }, consumerConfig.GroupId);
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

        protected abstract CompetingConsumerConfig GetConfig(CompetingConsumerConfig commonConfig);

        protected abstract Task Handle(string topic, TKey key, TValue value, IServiceProvider provider, CancellationToken cancellationToken = default);
    }
}
