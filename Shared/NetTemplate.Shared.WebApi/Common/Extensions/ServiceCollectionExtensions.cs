using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using NetTemplate.Shared.Infrastructure.Common.Extensions;
using NetTemplate.Shared.WebApi.Common.Filters;
using NetTemplate.Shared.WebApi.Common.Models;
using NetTemplate.Shared.WebApi.Identity.Extensions;
using NetTemplate.Shared.WebApi.Swagger.Extensions;
using VersioningConstants = NetTemplate.Shared.WebApi.Common.Constants.Versioning;

namespace NetTemplate.Shared.WebApi.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiVersioningDefaults(this IServiceCollection services)
        {
            return services.AddApiVersioning(opt =>
            {
                opt.ApiVersionReader = new UrlSegmentApiVersionReader();
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
            })
                .AddVersionedApiExplorer(opt =>
                {
                    opt.GroupNameFormat = VersioningConstants.GroupNameFormat;
                    //opt.SubstituteApiVersionInUrl = true;
                });
        }

        public static IServiceCollection ConfigureApiBehavior(this IServiceCollection services)
        {
            return services.Configure<ApiBehaviorOptions>(opt =>
            {
                // [IMPORTANT] Disable automatic 400 response
                opt.SuppressModelStateInvalidFilter = true;
            });
        }

        public static IMvcBuilder AddControllersDefaults(this IServiceCollection services,
            Action<MvcOptions> extraAction)
        {
            return services.AddControllers(opt =>
            {
                opt.Filters.Add<ValidateModelStateFilter>();
                opt.Filters.Add<ApiExceptionFilter>();
                opt.Filters.Add<ApiResponseWrapperFilter>();

                extraAction(opt);
            });
        }

        public static IServiceCollection AddApiDefaultServices<T>(this IServiceCollection services,
            IWebHostEnvironment environment,
            ApiDefaultServicesConfig config) where T : DbContext
        {
            bool isProduction = environment.IsProduction();

            services.AddDefaultServices<T>(config, isProduction)
                .AddHttpContextAccessor()
                .AddResponseCaching() // [OPTIONAL]
                .AddApiVersioningDefaults();

            if (!isProduction)
            {
                services.AddSwaggerDefaults();
            }

            services
                .AddAuthenticationDefaults(config.JwtConfig, config.ClientsConfig, environment)
                .AddAuthorizationDefaults();

            services
                .AddEndpointsApiExplorer()
                .ConfigureApiBehavior()
                .AddControllersDefaults(config.ControllerConfigureAction)
                .AddNewtonsoftJson();

            return services;
        }
    }
}
