using Microsoft.Extensions.Configuration;
using NetTemplate.Shared.ClientSDK.Common.Models;

namespace NetTemplate.Shared.Infrastructure.Common.Extensions
{
    public static class ConfigurationExtensions
    {
        public static ClientConfig GetClientConfigurationDefaults(this IConfiguration configuration)
            => configuration
                .GetSection(Constants.ConfigKeys.ClientSDK.ClientConfiguration)
                .Get<ClientConfig>();
    }
}
