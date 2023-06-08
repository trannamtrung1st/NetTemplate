using Microsoft.Extensions.Configuration;
using NetTemplate.Redis.Models;

namespace NetTemplate.Redis.Extensions
{
    public static class ConfigurationExtensions
    {
        public static RedisConfig GetRedisConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(Constants.ConfigurationSections.Redis).Get<RedisConfig>();

        public static RedisPubSubConfig GetRedisPubSubConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(Constants.ConfigurationSections.PubSub).Get<RedisPubSubConfig>();
    }
}
