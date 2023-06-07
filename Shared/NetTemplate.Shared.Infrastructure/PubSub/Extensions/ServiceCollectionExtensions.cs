using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using NetTemplate.ApacheKafka.Utils;
using NetTemplate.Shared.Infrastructure.PubSub.Models;

namespace NetTemplate.Shared.Infrastructure.PubSub.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPubSubIntegration(this IServiceCollection services, PubSubConfig pubSubConfig)
        {
            return services.AddSingleton(e =>
            {
                IAdminClient adminClient = KafkaHelper.CreateAdmin(pubSubConfig.AdminConfig);

                return adminClient;
            });
        }
    }
}
