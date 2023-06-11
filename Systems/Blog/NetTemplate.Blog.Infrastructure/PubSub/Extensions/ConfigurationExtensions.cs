using Microsoft.Extensions.Configuration;
using NetTemplate.Blog.Infrastructure.PubSub.Models;

namespace NetTemplate.Blog.Infrastructure.PubSub.Extensions
{
    public static class ConfigurationExtensions
    {
        public static PubSubConfig GetPubSubConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(Constants.ConfigurationSections.PubSub).Get<PubSubConfig>();
    }
}
