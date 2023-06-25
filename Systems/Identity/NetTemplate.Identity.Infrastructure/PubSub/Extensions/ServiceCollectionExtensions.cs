using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Identity.Infrastructure.Common.Implementations;
using NetTemplate.Identity.Infrastructure.PubSub.Models;
using NetTemplate.Redis.Extensions;
using NetTemplate.Redis.Models;
using NetTemplate.Shared.Infrastructure.PubSub.Implementations;
using NetTemplate.Shared.Infrastructure.PubSub.Interfaces;

namespace NetTemplate.Identity.Infrastructure.PubSub.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPubSub(this IServiceCollection services,
            PubSubConfig pubSubConfig,
            RedisPubSubConfig redisPubSubConfig = null)
        {
            if (pubSubConfig.UseKafka)
            {
                services.AddSingleton<ITopicManager, KafkaTopicManager>();
            }
            else if (pubSubConfig.UseRedis)
            {
                services.AddRedisPubSub(redisPubSubConfig)
                    .AddSingleton<ITopicManager, NullTopicManager>();
            }

            return services.AddSingleton<ITopicListenerManager, TopicListenerManager>();
        }
    }
}
