using Autofac;
using Autofac.Extras.DynamicProxy;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Common.Logging.Interceptors;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using System.Reflection;

namespace NetTemplate.Shared.Infrastructure.Common.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterApplicationServices(this ContainerBuilder builder, Assembly[] assemblies)
        {
            var interceptorTypes = new[] { typeof(AttributeBasedLoggingInterceptor) };

            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => type.IsClass)
                .AssignableTo<IDomainService>()
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(interceptorTypes)
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => type.IsClass)
                .AssignableTo<IDomainService>()
                .AsSelf()
                .EnableClassInterceptors()
                .InterceptedBy(interceptorTypes)
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => type.IsDefined(typeof(SelfTransientServiceAttribute), false))
                .AsSelf()
                .EnableClassInterceptors()
                .InterceptedBy(interceptorTypes)
                .InstancePerDependency();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => type.IsDefined(typeof(SelfScopedServiceAttribute), false))
                .AsSelf()
                .EnableClassInterceptors()
                .InterceptedBy(interceptorTypes)
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => type.IsDefined(typeof(SelfSingletonServiceAttribute), false))
                .AsSelf()
                .EnableClassInterceptors()
                .InterceptedBy(interceptorTypes)
                .SingleInstance();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => type.IsDefined(typeof(TransientServiceAttribute), false))
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(interceptorTypes)
                .InstancePerDependency();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => type.IsDefined(typeof(ScopedServiceAttribute), false))
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(interceptorTypes)
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => type.IsDefined(typeof(SingletonServiceAttribute), false))
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(interceptorTypes)
                .SingleInstance();

            return builder;
        }
    }
}
