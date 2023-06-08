using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.Infrastructure.Identity.Models;

namespace NetTemplate.Shared.Infrastructure.Identity.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IdentityConfig identityConfig)
        {
            // [TODO]

            services.ConfigureCopyableConfig(identityConfig);

            return services;
        }
    }
}
