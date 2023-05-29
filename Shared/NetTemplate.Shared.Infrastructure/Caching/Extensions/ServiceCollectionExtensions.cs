using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Shared.ApplicationCore.Caching.Interfaces;
using NetTemplate.Shared.Infrastructure.Caching.Implementations;

namespace NetTemplate.Shared.Infrastructure.Caching.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCaching(this IServiceCollection services)
        {
            return services.AddMemoryCache()
                .AddEasyCaching(options =>
                {
                    options.UseInMemory();
                })
                .AddSingleton<ApplicationCache>()
                .AddSingleton<IApplicationCache>(provider => provider.GetRequiredService<ApplicationCache>());
        }
    }
}
