using NetTemplate.Shared.WebApi.Common.Constants;
using NetTemplate.Shared.WebApi.Identity.Models;

namespace NetTemplate.Shared.WebApi.Identity.Extensions
{
    public static class ConfigurationExtensions
    {
        public static JwtConfig GetJwtConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(SharedApiConstants.ConfigKeys.Identity.DefaultJwtSection).Get<JwtConfig>();

        public static ClientsConfig GetClientsConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(SharedApiConstants.ConfigKeys.Identity.DefaultClientsConfig).Get<ClientsConfig>();
    }
}
