using Microsoft.Extensions.Configuration;
using NetTemplate.Blog.Infrastructure.Common.Models;

namespace NetTemplate.Blog.Infrastructure.Common.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T GetApplicationConfigDefaults<T>(this IConfiguration configuration)
            where T : ApplicationConfig
            => configuration
                .GetSection(ApplicationConfig.ConfigurationSection)
                .Get<T>();
    }
}
