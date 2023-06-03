using Microsoft.Extensions.Configuration;
using NetTemplate.Redis.Models;

namespace NetTemplate.Redis.Extensions
{
    public static class ConfigurationExtensions
    {
        public static RedisConfig GetRedisConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(Constants.DefaultConfigurationSection).Get<RedisConfig>();
    }
}
