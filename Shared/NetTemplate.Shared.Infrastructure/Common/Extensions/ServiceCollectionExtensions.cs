using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.ClientSDK.Common.Extensions;
using NetTemplate.Shared.ClientSDK.Common.Models;
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
            ClientConfiguration clientConfig)
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
    }
}
