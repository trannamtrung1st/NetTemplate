using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetTemplate.ApacheKafka.Extensions;
using NetTemplate.ApacheKafka.Models;
using NetTemplate.Common.DependencyInjection.Extensions;
using NetTemplate.Identity.Infrastructure.Common.Models;
using NetTemplate.Identity.Infrastructure.Persistence;
using NetTemplate.Identity.Infrastructure.PubSub.Extensions;
using NetTemplate.Identity.Infrastructure.PubSub.Models;
using NetTemplate.Redis.Extensions;
using NetTemplate.Redis.Models;
using NetTemplate.Shared.ClientSDK.Common.Models;
using NetTemplate.Shared.Infrastructure.Background.Models;
using NetTemplate.Shared.Infrastructure.Common.Extensions;
using NetTemplate.Shared.Infrastructure.Identity.Models;

namespace NetTemplate.Identity.Infrastructure.Common.Extensions
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
            ApacheKafkaConfig apacheKafkaConfig,
            PubSubConfig pubSubConfig)
        {
            services.AddInfrastructureDefaultServices<MainDbContext>(isProduction,
                dbContextConnectionString, appConfig.DbContextDebugEnabled,
                identityConfig,
                hangfireConfig, hangfireConnectionString, hangfireMasterConnectionString,
                runtimeConfig.ScanningAssemblies,
                redisConfig,
                clientConfig);

            services.ConfigureCopyableConfig(appConfig)
                .ConfigureCopyableConfig(runtimeConfig);

            if (redisConfig.Enabled)
            {
                services.AddRedisServices(redisConfig)
                    .AddRedisSimpleLock();
            }

            if (apacheKafkaConfig.Enabled)
            {
                services.AddKafkaServices(apacheKafkaConfig);
            }

            services.AddPubSub(pubSubConfig, redisPubSubConfig);

            return services;
        }

        public static IServiceCollection AddKafkaServices(this IServiceCollection services,
            ApacheKafkaConfig apacheKafkaConfig)
        {
            return services.AddKafka(apacheKafkaConfig);
        }

        public static IServiceCollection AddRedisServices(this IServiceCollection services,
            RedisConfig redisConfig)
        {
            return services.AddRedis(redisConfig)
                .AddRedisMemoryStore();
        }
    }
}
