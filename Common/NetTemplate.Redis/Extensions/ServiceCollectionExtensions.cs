using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Common.MemoryStore.Interfaces;
using NetTemplate.Redis.Implementations;
using NetTemplate.Redis.Models;
using NetTemplate.Redis.Utils;

namespace NetTemplate.Redis.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, RedisConfig redisConfig)
        {
            return services.AddSingleton(e => RedisHelper.GetConnectionMultiplexer(redisConfig))
                .ConfigureCopyableConfig(redisConfig);
        }

        public static IServiceCollection AddRedisPubSub(this IServiceCollection services, RedisPubSubConfig redisPubSubConfig)
        {
            return services.ConfigureCopyableConfig(redisPubSubConfig);
        }

        public static IServiceCollection AddRedisMemoryStore(this IServiceCollection services)
        {
            return services.AddSingleton<IDistributedMemoryStore, RedisMemoryStore>();
        }
    }
}
