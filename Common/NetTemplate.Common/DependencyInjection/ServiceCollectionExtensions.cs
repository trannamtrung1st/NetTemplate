using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Common.Objects;

namespace NetTemplate.Common.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureCopyableConfig<T>(this IServiceCollection services,
            T currentConfig)
            where T : class, ICopyable<T>
        {
            return services.Configure<T>(config => currentConfig.CopyTo(config));
        }
    }
}
