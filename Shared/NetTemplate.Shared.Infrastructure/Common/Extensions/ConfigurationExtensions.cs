using Microsoft.Extensions.Configuration;
using NetTemplate.Shared.ClientSDK.Common.Models;

namespace NetTemplate.Shared.Infrastructure.Common.Extensions
{
    public static class ConfigurationExtensions
    {
        public static ClientConfig GetClientConfigDefaults(this IConfiguration configuration)
            => configuration
                .GetSection(Constants.ConfigurationSections.ClientSDK)
                .Get<ClientConfig>();
    }
}
