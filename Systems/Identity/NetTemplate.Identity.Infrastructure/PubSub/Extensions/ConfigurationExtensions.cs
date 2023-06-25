using Microsoft.Extensions.Configuration;
using NetTemplate.Identity.Infrastructure.PubSub.Models;

namespace NetTemplate.Identity.Infrastructure.PubSub.Extensions
{
    public static class ConfigurationExtensions
    {
        public static PubSubConfig GetPubSubConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(PubSubConfig.ConfigurationSection).Get<PubSubConfig>();
    }
}
