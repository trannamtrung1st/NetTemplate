using Microsoft.Extensions.Configuration;
using NetTemplate.Shared.ClientSDK.Common.Models;

namespace NetTemplate.Shared.Infrastructure.Common.Extensions
{
    public static class ConfigurationExtensions
    {
        public static ClientConfiguration GetClientConfigurationDefaults(this IConfiguration configuration)
            => configuration
                .GetSection(CommonConstants.ConfigKeys.ClientSDK.DefaultClientConfiguration)
                .Get<ClientConfiguration>();
    }
}
