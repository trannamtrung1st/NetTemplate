using EasyCaching.Core.Configurations;
using EasyCaching.Redis;
using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Redis.Models;
using NetTemplate.Shared.ApplicationCore.Caching.Interfaces;
using NetTemplate.Shared.Infrastructure.Caching.Implementations;
using CachingProviders = NetTemplate.Shared.Infrastructure.Caching.Constants.CachingProviders;

namespace NetTemplate.Shared.Infrastructure.Caching.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCaching(this IServiceCollection services,
            RedisConfig redisConfig = null)
        {
            return services.AddMemoryCache()
                .AddEasyCaching(options =>
                {
                    options.UseInMemory(name: CachingProviders.InMemory);

                    if (redisConfig?.Enabled == true)
                    {
                        options.WithJson(name: CachingProviders.Redis);

                        options.UseRedis(redisOpt =>
                        {
                            redisOpt.SerializerName = CachingProviders.Redis;
                            RedisDBOptions dbConfig = redisOpt.DBConfig;
                            dbConfig.Username = redisConfig.User;
                            dbConfig.Password = redisConfig.Password;

                            foreach (string endPoint in redisConfig.Endpoints)
                            {
                                string[] parts = endPoint.Split(':');
                                string host = parts[0];
                                int port = parts.Length > 1 ? int.Parse(parts[1]) : 6379;
                                dbConfig.Endpoints.Add(new ServerEndPoint(host, port));
                            }
                        }, name: CachingProviders.Redis);
                    }
                })
                .AddSingleton<IApplicationCache, ApplicationCache>();
        }
    }
}
