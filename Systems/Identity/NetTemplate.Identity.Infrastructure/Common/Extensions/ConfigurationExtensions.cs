using Microsoft.Extensions.Configuration;
using NetTemplate.Identity.Infrastructure.Common.Models;

namespace NetTemplate.Identity.Infrastructure.Common.Extensions
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
