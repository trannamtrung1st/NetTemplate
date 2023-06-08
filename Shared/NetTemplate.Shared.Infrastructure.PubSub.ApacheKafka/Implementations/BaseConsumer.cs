using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetTemplate.ApacheKafka.Utils;
using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Interfaces;
using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Models;

namespace NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Implementations
{
    public abstract class BaseConsumer<TConsumer, TKey, TValue> : IGeneralConsumer
    {
        protected readonly IServiceProvider provider;
        protected readonly IConfiguration configuration;
        protected readonly ILogger<TConsumer> logger;

        public BaseConsumer(
            IServiceProvider provider,
            IConfiguration configuration,
            ILogger<TConsumer> logger)
        {
            this.provider = provider;
            this.configuration = configuration;
            this.logger = logger;
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
            using IConsumer<TKey, TValue> consumer = KafkaHelper.CreateConsumer<TKey, TValue>(consumerConfig);

            try
            {
                consumer.Subscribe(Topics);

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
