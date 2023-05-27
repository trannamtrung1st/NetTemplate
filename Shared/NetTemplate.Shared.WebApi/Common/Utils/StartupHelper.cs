using Autofac;
using Microsoft.EntityFrameworkCore;
using NetTemplate.Shared.Infrastructure.Background.Extensions;
using NetTemplate.Shared.Infrastructure.Caching.Extensions;
using NetTemplate.Shared.Infrastructure.Common.Extensions;
using NetTemplate.Shared.Infrastructure.Logging.Extensions;
using NetTemplate.Shared.Infrastructure.Persistence.Extensions;
using NetTemplate.Shared.Infrastructure.PubSub.Extensions;
using NetTemplate.Shared.Infrastructure.Resilience.Extensions;
using NetTemplate.Shared.Infrastructure.Validation.Extensions;
using NetTemplate.Shared.WebApi.Common.Extensions;
using NetTemplate.Shared.WebApi.Common.Models;
using NetTemplate.Shared.WebApi.Identity.Extensions;
using NetTemplate.Shared.WebApi.Swagger.Extensions;
using System.Reflection;

namespace NetTemplate.Shared.WebApi.Common.Utils
{
    public static class StartupHelper
    {
        public static void AddDefaultServices<T>(this IServiceCollection services,
            IWebHostEnvironment environment,
            DefaultServicesConfig config) where T : DbContext
        {
            services
                .AddDbContextDefaults<T>(config.DbContextConnectionString, config.DbContextDebugEnabled)
                .AddIdentityConfiguration(config.IdentityConfig)
                .AddPubSubIntegration(config.PubSubConfig)
                .AddHangfireDefaults(config.HangfireConfig, config.HangfireConnectionString, config.HangfireMasterConnectionString)
                .AddMediator(config.ScanningAssemblies)
                .AddMapper(config.ScanningAssemblies)
                .AddValidationDefaults(config.ScanningAssemblies)
                .AddHttpContextAccessor()
                .AddResponseCaching() // [OPTIONAL]
                .AddCaching()
                .AddResilience()
                .AddApiVersioningDefaults()
                .AddClientSdkServices(config.ClientConfig);

            if (!environment.IsProduction())
            {
                services
                    .AddSwaggerDefaults()
                    .AddLoggingInterceptors();
            }

            services
                .AddAuthenticationDefaults(config.JwtConfig, config.ClientsConfig, environment)
                .AddAuthorizationDefaults();

            services
                .AddEndpointsApiExplorer()
                .ConfigureApiBehavior()
                .AddControllersDefaults(config.ControllerConfigureAction)
                .AddNewtonsoftJson();
        }

        public static void ConfigureAutofacContainerDefaults(this ContainerBuilder builder,
            Assembly[] scanningAssemblies)
        {
            builder.RegisterApplicationServices(scanningAssemblies);
        }

        public static void CleanResources(IEnumerable<IDisposable> resources)
        {
            foreach (var resource in resources)
                resource.Dispose();
        }
    }
}
