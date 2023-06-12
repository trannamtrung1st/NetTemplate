using Microsoft.AspNetCore.Mvc;
using NetTemplate.Blog.Infrastructure.Persistence;
using NetTemplate.Blog.WebApi.Common.Models;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.WebApi.Common.Extensions;
using NetTemplate.Shared.WebApi.Identity.Models;

namespace NetTemplate.Blog.WebApi.Common.Extensions
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
