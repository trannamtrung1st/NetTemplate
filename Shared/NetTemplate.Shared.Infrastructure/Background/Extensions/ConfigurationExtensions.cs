using Microsoft.Extensions.Configuration;
using NetTemplate.Shared.Infrastructure.Background.Models;

namespace NetTemplate.Shared.Infrastructure.Background.Extensions
{
    public static class ConfigurationExtensions
    {
        public static HangfireConfig GetHangfireConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(Constants.ConfigurationSection).Get<HangfireConfig>();
    }
}
