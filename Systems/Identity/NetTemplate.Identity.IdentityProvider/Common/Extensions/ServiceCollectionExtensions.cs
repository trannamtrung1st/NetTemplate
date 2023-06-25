using Microsoft.AspNetCore.Mvc;
using NetTemplate.Common.DependencyInjection.Extensions;
using NetTemplate.Identity.IdentityProvider.Common.Models;
using NetTemplate.Identity.Infrastructure.Persistence;
using NetTemplate.Shared.WebApi.Common.Extensions;
using NetTemplate.Shared.WebApi.Identity.Models;

namespace NetTemplate.Identity.IdentityProvider.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IWebHostEnvironment env,
            WebApplicationConfig webConfig,
            JwtConfig jwtConfig,
            ApplicationClientsConfig clientsConfig,
            SimulatedAuthConfig simulatedAuthConfig,
            Action<MvcOptions> controllerConfigureAction)
        {
            services.AddApiDefaultServices<MainDbContext>(env,
                jwtConfig,
                clientsConfig,
                simulatedAuthConfig,
                controllerConfigureAction);

            services.ConfigureCopyableConfig(webConfig);

            return services;
        }
    }
}
