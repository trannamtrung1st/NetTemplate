using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace NetTemplate.Shared.Infrastructure.Validation.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddValidationDefaults(this IServiceCollection services, Assembly[] assemblies)
        {
            ValidatorOptions.Global.DisplayNameResolver = (type, member, expression) =>
                ValidatorOptions.Global.PropertyNameResolver(type, member, expression);

            return services
                .AddValidatorsFromAssemblies(assemblies);
        }
    }
}
