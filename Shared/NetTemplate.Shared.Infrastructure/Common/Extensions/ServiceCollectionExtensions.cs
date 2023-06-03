using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Common.MemoryStore.Extensions;
using NetTemplate.Shared.ApplicationCore.Common.Extensions;
using NetTemplate.Shared.ApplicationCore.Identity.Extensions;
using NetTemplate.Shared.ClientSDK.Common.Extensions;
using NetTemplate.Shared.ClientSDK.Common.Models;
using NetTemplate.Shared.Infrastructure.Background.Extensions;
using NetTemplate.Shared.Infrastructure.Caching.Extensions;
using NetTemplate.Shared.Infrastructure.Common.Models;
using NetTemplate.Shared.Infrastructure.Identity.Extensions;
using NetTemplate.Shared.Infrastructure.Logging.Extensions;
using NetTemplate.Shared.Infrastructure.Persistence.Extensions;
using NetTemplate.Shared.Infrastructure.PubSub.Extensions;
using NetTemplate.Shared.Infrastructure.Resilience.Extensions;
using NetTemplate.Shared.Infrastructure.Validation.Extensions;
using System.Reflection;

namespace NetTemplate.Shared.Infrastructure.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services,
            Assembly[] assemblies)
        {
            services.Scan(scan => scan.FromApplicationDependencies(
                    assembly => assembly.GetName()?.Name?.StartsWith(nameof(NetTemplate)) == true)
                .AddClasses(classes => classes
                    .Where(c => !c.IsAbstract)
                    .AssignableTo(typeof(IPipelineBehavior<,>)))
                .As(typeof(IPipelineBehavior<,>))
                .WithScopedLifetime());

            return services.AddMediatR(config =>
            {
                config.Lifetime = ServiceLifetime.Scoped;
                config.RegisterServicesFromAssemblies(assemblies);
            });
        }

        public static IServiceCollection AddClientSdkServices(this IServiceCollection services,
            ClientConfig clientConfig)
        {
            services.AddClientSdkHandlers()
                .AddTokenManagement(clientConfig);

            return services;
        }

        public static IServiceCollection AddMapper(this IServiceCollection services,
            Assembly[] assemblies)
        {
            return services.AddAutoMapper(assemblies);
        }

        public static IServiceCollection ScanServices(this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
        {
            return services.Scan(scan => scan.FromAssemblies(assemblies)
                .AddClasses(classes => classes.WithAttribute<TransientServiceAttribute>())
                .AsSelfWithInterfaces()
                .WithTransientLifetime()

                .AddClasses(classes => classes.WithAttribute<ScopedServiceAttribute>())
                .AsSelfWithInterfaces()
                .WithScopedLifetime()

                .AddClasses(classes => classes.WithAttribute<SingletonServiceAttribute>())
                .AsSelfWithInterfaces()
                .WithSingletonLifetime()

                .AddClasses(classes => classes.WithAttribute<SelfTransientServiceAttribute>())
                .AsSelf()
                .WithTransientLifetime()

                .AddClasses(classes => classes.WithAttribute<SelfScopedServiceAttribute>())
                .AsSelf()
                .WithScopedLifetime()

                .AddClasses(classes => classes.WithAttribute<SelfSingletonServiceAttribute>())
                .AsSelf()
                .WithSingletonLifetime());
        }

        public static IServiceCollection AddInfrastructureDefaultServices<T>(this IServiceCollection services,
            DefaultServicesConfig config, bool isProduction) where T : DbContext
        {
            services
                .AddDbContextDefaults<T>(config.DbContextConnectionString, config.DbContextDebugEnabled)
                .AddIdentityConfiguration(config.IdentityConfig)
                .AddPubSubIntegration(config.PubSubConfig)
                .AddHangfireDefaults(config.HangfireConfig, config.HangfireConnectionString, config.HangfireMasterConnectionString)
                .AddMediator(config.ScanningAssemblies)
                .AddMapper(config.ScanningAssemblies)
                .AddValidationDefaults(config.ScanningAssemblies)
                .AddCaching(useRedis: config.UseRedis, redisConfig: config.RedisConfig)
                .AddResilience()
                .AddSimpleMemoryStore()
                .AddClientSdkServices(config.ClientConfig)
                .AddNullCurrentUserProvider()
                .AddEntityVersionManager();

            if (!isProduction)
            {
                services.AddLoggingInterceptors();
            }

            return services;
        }
    }
}
