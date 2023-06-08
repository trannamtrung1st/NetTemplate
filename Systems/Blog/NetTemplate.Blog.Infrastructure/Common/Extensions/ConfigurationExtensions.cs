using Microsoft.Extensions.Configuration;
using NetTemplate.Blog.Infrastructure.Common.Models;
using ConfigurationSections = NetTemplate.Blog.Infrastructure.Common.Constants.ConfigurationSections;

namespace NetTemplate.Blog.Infrastructure.Common.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T GetApplicationConfigDefaults<T>(this IConfiguration configuration)
            where T : ApplicationConfig
            => configuration
                .GetSection(ConfigurationSections.Application)
                .Get<T>();
    }
}
