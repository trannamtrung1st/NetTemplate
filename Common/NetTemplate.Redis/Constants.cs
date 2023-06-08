using NetTemplate.Redis.Models;

namespace NetTemplate.Redis
{
    public static class Constants
    {
        public static class ConfigurationSections
        {
            public const string Redis = nameof(RedisConfig);
            public const string PubSub = nameof(RedisPubSubConfig);
        }
    }
}
