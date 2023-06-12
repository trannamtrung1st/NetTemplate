using NetTemplate.Shared.WebApi.Common.Models;

namespace NetTemplate.Shared.WebApi.Common.Extensions
{
    public static class ConfigurationExtensions
    {
        public static WebInfoConfig GetWebInfoConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(WebInfoConfig.ConfigurationSection).Get<WebInfoConfig>();
    }
}
