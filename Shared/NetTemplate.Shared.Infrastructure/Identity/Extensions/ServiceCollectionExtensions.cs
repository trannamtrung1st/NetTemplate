using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Shared.Infrastructure.Identity.Models;

namespace NetTemplate.Shared.Infrastructure.Identity.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IdentityConfig identityConfig)
        {
            // [TODO]

            return services;
        }
    }
}
