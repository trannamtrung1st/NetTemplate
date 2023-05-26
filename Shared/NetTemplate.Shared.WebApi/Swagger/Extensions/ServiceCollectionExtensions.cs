using NetTemplate.Shared.WebApi.Swagger.Options;

namespace NetTemplate.Shared.WebApi.Swagger.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerDefaults(this IServiceCollection services)
        {
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerGen();
            services.ConfigureOptions<ConfigureSwaggerGenOptions>();
            return services;
        }
    }
}
