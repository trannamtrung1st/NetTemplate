using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Blog.Infrastructure.Common.Implementations;
using NetTemplate.Blog.Infrastructure.Domains.User.Interfaces;
using NetTemplate.Blog.Infrastructure.Domains.User.Listeners;
using NetTemplate.Blog.Infrastructure.PubSub.Models;
using NetTemplate.Redis.Extensions;
using NetTemplate.Redis.Models;
using NetTemplate.Shared.Infrastructure.PubSub.Implementations;
using NetTemplate.Shared.Infrastructure.PubSub.Interfaces;

namespace NetTemplate.Blog.Infrastructure.PubSub.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPubSub(this IServiceCollection services,
            PubSubConfig pubSubConfig,
            RedisPubSubConfig redisPubSubConfig = null)
        {
            if (pubSubConfig.UseKafka)
            {
                services.AddSingleton<ITopicManager, KafkaTopicManager>()
                    .AddSingleton<ISyncNewUserListener, SyncNewUserKafkaListener>();
            }
            else if (pubSubConfig.UseRedis)
            {
                services.AddRedisPubSub(redisPubSubConfig)
                    .AddSingleton<ITopicManager, NullTopicManager>()
                    .AddSingleton<ISyncNewUserListener, SyncNewUserRedisListener>();
            }

            return services.AddSingleton<ITopicListenerManager, TopicListenerManager>();
        }
    }
}
