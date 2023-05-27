using NetTemplate.Shared.WebApi.Common.Models;
using ConfigurationSections = NetTemplate.Shared.WebApi.Common.Constants.ConfigurationSections;

namespace NetTemplate.Shared.WebApi.Common.Extensions
{
    public static class ConfigurationExtensions
    {
        public static WebInfoConfig GetWebInfoConfigDefaults(this IConfiguration configuration)
            => configuration
                .GetSection(ConfigurationSections.Common.WebInfo)
                .Get<WebInfoConfig>();
    }
}
