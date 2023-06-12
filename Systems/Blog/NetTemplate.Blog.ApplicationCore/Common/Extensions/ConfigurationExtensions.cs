using Microsoft.Extensions.Configuration;
using NetTemplate.Blog.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.Common.Extensions
{
    public static class ConfigurationExtensions
    {
        public static ViewsConfig GetViewsConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(ViewsConfig.ConfigurationSection).Get<ViewsConfig>();

    }
}
