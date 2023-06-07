using Microsoft.Extensions.Configuration;
using NetTemplate.Shared.Infrastructure.PubSub.Models;

namespace NetTemplate.Shared.Infrastructure.PubSub.Extensions
{
    public static class ConfigurationExtensions
    {
        public static PubSubConfig GetPubSubConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(Constants.ConfigurationSection).Get<PubSubConfig>();
    }
}
