using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Common.Logging.Interceptors;

namespace NetTemplate.Shared.Infrastructure.Logging.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLoggingInterceptors(this IServiceCollection services)
        {
            return services.AddScoped<MethodLoggingInterceptor>()
                .AddScoped<AttributeBasedLoggingInterceptor>();
        }
    }
}
