﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetTemplate.ApacheKafka.Extensions;
using NetTemplate.ApacheKafka.Models;
using NetTemplate.Blog.ApplicationCore.Common.Extensions;
using NetTemplate.Blog.Infrastructure.Common.Implementations;
using NetTemplate.Blog.Infrastructure.Common.Interfaces;
using NetTemplate.Blog.Infrastructure.Common.Models;
using NetTemplate.Blog.Infrastructure.Domains.Post.Extensions;
using NetTemplate.Blog.Infrastructure.Persistence;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Redis.Extensions;
using NetTemplate.Redis.Models;
using NetTemplate.Shared.ClientSDK.Common.Models;
using NetTemplate.Shared.Infrastructure.Background.Models;
using NetTemplate.Shared.Infrastructure.Common.Extensions;
using NetTemplate.Shared.Infrastructure.Identity.Models;

namespace NetTemplate.Blog.Infrastructure.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration, bool isProduction,
            RuntimeConfig runtimeConfig, ApplicationConfig appConfig,
            string dbContextConnectionString,
            IdentityConfig identityConfig,
            HangfireConfig hangfireConfig, string hangfireConnectionString, string hangfireMasterConnectionString,
            RedisConfig redisConfig, RedisPubSubConfig redisPubSubConfig,
            ClientConfig clientConfig,
            ApacheKafkaConfig apacheKafkaConfig)
        {
            services.AddInfrastructureDefaultServices<MainDbContext>(isProduction,
                dbContextConnectionString, appConfig.DbContextDebugEnabled,
                identityConfig,
                hangfireConfig, hangfireConnectionString, hangfireMasterConnectionString,
                runtimeConfig.ScanningAssemblies,
                redisConfig,
                clientConfig);

            services.ConfigureViewConfigs(configuration)
                .ConfigureCopyableConfig(appConfig)
                .ConfigureCopyableConfig(runtimeConfig);

            if (redisConfig.Enabled)
            {
                services.AddRedisServices(redisConfig, redisPubSubConfig)
                    .AddSingleton<ITopicManager, NullTopicManager>()
                    .AddSingleton<ITopicListenerManager, RedisSubscriberManager>();
            }
            else
            {
                services.AddSimplePostCache();
            }

            if (apacheKafkaConfig.Enabled)
            {
                services.AddKafkaServices(apacheKafkaConfig)
                    .AddSingleton<ITopicManager, KafkaTopicManager>()
                    .AddSingleton<ITopicListenerManager, KafkaConsumerManager>();
            }

            return services;
        }

        public static IServiceCollection AddKafkaServices(this IServiceCollection services,
            ApacheKafkaConfig apacheKafkaConfig)
        {
            return services.AddKafka(apacheKafkaConfig);
        }

        public static IServiceCollection AddRedisServices(this IServiceCollection services,
            RedisConfig redisConfig, RedisPubSubConfig redisPubSubConfig)
        {
            return services.AddRedis(redisConfig)
                .AddRedisMemoryStore()
                .AddRedisPostCache()
                .AddRedisPubSub(redisPubSubConfig);
        }
    }
}
