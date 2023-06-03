using NetTemplate.Redis.Models;
using StackExchange.Redis;

namespace NetTemplate.Redis.Utils
{
    public static class RedisHelper
    {
        public static ConnectionMultiplexer GetConnectionMultiplexer(RedisConfig redisConfig)
        {
            ConfigurationOptions cfg = new ConfigurationOptions
            {
                User = redisConfig.User,
                Password = redisConfig.Password
            };

            foreach (string endPoint in redisConfig.Endpoints)
            {
                cfg.EndPoints.Add(endPoint);
            }

            string connStr = cfg.ToString();

            return ConnectionMultiplexer.Connect(connStr);
        }
    }
}
