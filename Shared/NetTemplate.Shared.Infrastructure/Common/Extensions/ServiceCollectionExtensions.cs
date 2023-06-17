using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Common.DependencyInjection.Attributes;
using NetTemplate.Common.MemoryStore.Extensions;
using NetTemplate.Common.Synchronization.Extensions;
using NetTemplate.Redis.Models;
using NetTemplate.Shared.ApplicationCore.Common.Extensions;
using NetTemplate.Shared.ApplicationCore.Identity.Extensions;
using NetTemplate.Shared.ClientSDK.Common.Extensions;
using NetTemplate.Shared.ClientSDK.Common.Models;
using NetTemplate.Shared.Infrastructure.Background.Extensions;
using NetTemplate.Shared.Infrastructure.Background.Models;
using NetTemplate.Shared.Infrastructure.Caching.Extensions;
using NetTemplate.Shared.Infrastructure.Domains.Identity.Extensions;
using NetTemplate.Shared.Infrastructure.Identity.Extensions;
using NetTemplate.Shared.Infrastructure.Identity.Models;
using NetTemplate.Shared.Infrastructure.Logging.Extensions;
using NetTemplate.Shared.Infrastructure.Persistence.Extensions;
using NetTemplate.Shared.Infrastructure.Resilience.Extensions;
using NetTemplate.Shared.Infrastructure.Resilience.Utils;
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
            services.AddClientSdkDefaults(clientConfig);

            IHttpClientBuilder httpBuilder = services.AddTokenManagement(clientConfig);

            httpBuilder.AddPolicyHandlerFromRegistry(PollyHelper.TransientHttpErrorPolicy);

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

        public static IServiceCollection AddInfrastructureDefaultServices<T>(this IServiceCollection services, bool isProduction,
            string dbContextConnectionString, bool dbContextDebugEnabled,
            IdentityConfig identityConfig,
            HangfireConfig hangfireConfig, string hangfireConnectionString, string hangfireMasterConnectionString,
            Assembly[] scanningAssemblies,
            RedisConfig redisConfig,
            ClientConfig clientConfig) where T : DbContext
        {
            services
                .AddDbContextDefaults<T>(dbContextConnectionString, dbContextDebugEnabled)
                .AddIdentityServices(identityConfig)
                .AddHangfireDefaults(hangfireConfig, hangfireConnectionString, hangfireMasterConnectionString)
                .AddMediator(scanningAssemblies)
                .AddMapper(scanningAssemblies)
                .AddValidationDefaults(scanningAssemblies)
                .AddCaching(redisConfig)
                .AddResilience()
                .AddSimpleMemoryStore()
                .AddClientSdkServices(clientConfig)
                .AddIdentityService()
                .AddNullCurrentUserProvider()
                .AddEntityVersionManager()
                .AddSemaphoreSlimLock();

            if (!isProduction)
            {
                services.AddLoggingInterceptors();
            }

            return services;
        }
    }
}
