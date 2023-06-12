﻿using EasyCaching.Core.Configurations;
using EasyCaching.Redis;
using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Redis.Models;
using NetTemplate.Shared.ApplicationCore.Caching.Interfaces;
using NetTemplate.Shared.Infrastructure.Caching.Implementations;

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
                    options.UseInMemory(name: RedisCachingProvider);

                    if (redisConfig?.Enabled == true)
                    {
                        options.WithJson(name: RedisCachingProvider);

                        options.UseRedis(redisOpt =>
                        {
                            redisOpt.SerializerName = RedisCachingProvider;
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
                        }, name: RedisCachingProvider);
                    }
                })
                .AddSingleton<IApplicationCache, ApplicationCache>();
        }

        public const string InMemoryCachingProvider = "InMemory";
        public const string RedisCachingProvider = "Redis";
    }
}
