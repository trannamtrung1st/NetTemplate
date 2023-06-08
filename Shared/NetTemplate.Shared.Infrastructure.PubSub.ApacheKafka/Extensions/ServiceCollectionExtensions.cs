using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using NetTemplate.ApacheKafka.Utils;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Implementations;
using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Interfaces;
using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Models;

namespace NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafka(this IServiceCollection services, ApacheKafkaConfig kafkaConfig)
        {
            return services.AddSingleton(e =>
            {
                IAdminClient adminClient = KafkaHelper.CreateAdmin(kafkaConfig.AdminConfig);

                return adminClient;
            })
                .AddMemoryOffsetStore()
                .ConfigureCopyableConfig(kafkaConfig);
        }

        public static IServiceCollection AddMemoryOffsetStore(this IServiceCollection services)
            => services.AddScoped<IExternalOffsetStore, MemoryOffsetStore>();
    }
}
