using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Common.DependencyInjection;
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
    }
}
