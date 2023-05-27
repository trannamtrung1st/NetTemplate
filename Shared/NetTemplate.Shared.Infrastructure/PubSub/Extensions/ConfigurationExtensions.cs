using Microsoft.Extensions.Configuration;
using NetTemplate.Shared.Infrastructure.PubSub.Models;

namespace NetTemplate.Shared.Infrastructure.PubSub.Extensions
{
    public static class ConfigurationExtensions
    {
        // [TODO]
        public static PubSubConfig GetPubSubConfigDefaults(this IConfiguration configuration)
            => new PubSubConfig();
    }
}
