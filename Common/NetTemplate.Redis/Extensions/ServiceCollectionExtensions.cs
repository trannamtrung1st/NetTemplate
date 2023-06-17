using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Common.DependencyInjection.Extensions;
using NetTemplate.Common.MemoryStore.Interfaces;
using NetTemplate.Common.Synchronization.Interfaces;
using NetTemplate.Redis.Implementations;
using NetTemplate.Redis.Models;
using NetTemplate.Redis.Utils;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

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

        public static IServiceCollection AddRedisSimpleLock(this IServiceCollection services)
        {
            return services.AddSingleton<IDistributedLock, RedisSimpleLock>();
        }

        public static IServiceCollection AddRedLock(this IServiceCollection services,
            IEnumerable<ConnectionMultiplexer> connectionMultiplexers = null)
        {
            return services.AddSingleton(provider =>
            {
                List<RedLockMultiplexer> redLockMultiplexers = new List<RedLockMultiplexer>();

                if (connectionMultiplexers?.Any() == true)
                {
                    foreach (ConnectionMultiplexer connectionMultiplexer in connectionMultiplexers)
                    {
                        redLockMultiplexers.Add(connectionMultiplexer);
                    }
                }
                else
                {
                    ConnectionMultiplexer multiplexer = provider.GetRequiredService<ConnectionMultiplexer>();

                    redLockMultiplexers.Add(multiplexer);
                }

                return RedLockFactory.Create(redLockMultiplexers);
            }).AddSingleton<IDistributedLock, RedisRedLock>();
        }
    }
}
