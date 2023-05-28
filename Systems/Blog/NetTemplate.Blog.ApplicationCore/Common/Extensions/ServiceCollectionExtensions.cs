using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Blog.ApplicationCore.Common.Models;
using ConfigurationSections = NetTemplate.Blog.ApplicationCore.Common.Constants.ConfigurationSections;

namespace NetTemplate.Blog.ApplicationCore.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureViewConfigs(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.Configure<ViewsConfig>(opt =>
            {
                configuration.GetSection(ConfigurationSections.Views).Bind(opt);
            });
        }
    }
}
