using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Blog.ApplicationCore.Common.Models;
using NetTemplate.Common.DependencyInjection;

namespace NetTemplate.Blog.ApplicationCore.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureViewConfigs(this IServiceCollection services,
            ViewsConfig viewsConfig)
        {
            return services.ConfigureCopyableConfig(viewsConfig);
        }
    }
}
