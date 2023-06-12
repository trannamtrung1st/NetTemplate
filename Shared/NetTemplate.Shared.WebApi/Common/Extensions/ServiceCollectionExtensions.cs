using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using NetTemplate.Shared.WebApi.Common.Filters;
using NetTemplate.Shared.WebApi.Common.Middlewares;
using NetTemplate.Shared.WebApi.Identity.Extensions;
using NetTemplate.Shared.WebApi.Identity.Models;
using NetTemplate.Shared.WebApi.Swagger.Extensions;
using Newtonsoft.Json.Converters;

namespace NetTemplate.Shared.WebApi.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRequestDataExtraction(this IServiceCollection services)
            => services.AddScoped<RequestDataExtractionMiddleware>();

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
                    opt.GroupNameFormat = ApiVersionGroupNameFormat;
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
            JwtConfig jwtConfig,
            ApplicationClientsConfig applicationClientsConfig,
            SimulatedAuthConfig simulatedAuthConfig,
            Action<MvcOptions> controllerConfigureAction) where T : DbContext
        {
            bool isProduction = environment.IsProduction();

            services
                .AddRequestDataExtraction()
                .AddHttpContextAccessor()
                .AddResponseCaching() // [OPTIONAL]
                .AddApiVersioningDefaults();

            if (!isProduction)
            {
                services.AddSwaggerDefaults();
            }

            services
                .AddRequestCurrentUserProvider()
                .AddAuthenticationDefaults(jwtConfig, applicationClientsConfig, environment, simulatedAuthConfig)
                .AddAuthorizationDefaults();

            services
                .AddEndpointsApiExplorer()
                .ConfigureApiBehavior()
                .AddControllersDefaults(controllerConfigureAction)
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            return services;
        }


        public const string ApiVersionGroupNameFormat = "'v'VVV";
    }
}
