using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTemplate.Shared.Infrastructure.Background.Models;
using NetTemplate.Shared.Infrastructure.Persistence.Models;
using NetTemplate.Shared.WebApi.Extensions;
using NetTemplate.Shared.WebApi.Models;
using System.Reflection;

namespace NetTemplate.Shared.WebApi.Utils
{
    public class StartupHelper
    {
        public static void BindConfigurations(IConfiguration configuration)
        {
            configuration.Bind(nameof(AppConfig), AppConfig.Instance);
            configuration.Bind(nameof(IdentityConfig), IdentityConfig.Instance);
            configuration.Bind(nameof(DatabaseConfig), DatabaseConfig.Instance);
            configuration.Bind(nameof(SerilogConfig), SerilogConfig.Instance);
            configuration.Bind(nameof(HangfireConfig), HangfireConfig.Instance);
            configuration.Bind(nameof(ClientsConfig), ClientsConfig.Instance);
            configuration.Bind(nameof(JwtConfig), JwtConfig.Instance);
            configuration.Bind(nameof(WebApiConfig), WebApiConfig.Instance);
        }

        public static void AddCommonServices<T>(IServiceCollection services,
            IWebHostEnvironment environment,
            IConfiguration configuration, Assembly[] allAssembliesForScanning,
            Action<MvcOptions> controllerConfigureAct) where T : DbContext
        {
            services
                .AddDbContextDefaults<T>(configuration, "[TODO]")
                .AddIdentityConfiguration(configuration)
                .AddPubSubIntegration(configuration)
                .AddHangfireDefaults(configuration)
                .AddMediator(allAssembliesForScanning)
                .AddMapper(allAssembliesForScanning)
                .AddValidationDefaults(allAssembliesForScanning)
                .AddHttpContextAccessor()
                .AddResponseCaching() // [OPTIONAL]
                .AddCaching()
                .AddResilience()
                .AddApiVersioningDefaults()
                .AddClientSdkServices();

            if (!environment.IsProduction())
            {
                services
                    .AddSwaggerDefaults()
                    .AddLoggingInterceptors();
            }

            services
                .AddAuthenticationDefaults(environment)
                .AddAuthorizationDefaults();

            services
                .ConfigureApiBehavior()
                .AddControllersDefaults(opt =>
                {
                    controllerConfigureAct(opt);
                })
                .AddNewtonsoftJson();
        }

        public static void ConfigureCommonAutofacContainer(ContainerBuilder builder,
            Assembly[] allAssembliesForScanning)
        {
            builder.AddApplicationServices(allAssembliesForScanning);
        }

        public static void CleanUpCommonResources(IEnumerable<IDisposable> resources)
        {
            Console.WriteLine("Cleaning resources ...");

            foreach (var resource in resources)
                resource.Dispose();
        }
    }
}
