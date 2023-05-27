using NetTemplate.Shared.WebApi.Identity.Models;
using ConfigurationSections = NetTemplate.Shared.WebApi.Common.Constants.ConfigurationSections;

namespace NetTemplate.Shared.WebApi.Identity.Extensions
{
    public static class ConfigurationExtensions
    {
        public static JwtConfig GetJwtConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(ConfigurationSections.Identity.Jwt).Get<JwtConfig>();

        public static ClientsConfig GetClientsConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(ConfigurationSections.Identity.Clients).Get<ClientsConfig>();
    }
}
