using NetTemplate.Shared.WebApi.Identity.Models;
using IdentityConfigurationSections = NetTemplate.Shared.WebApi.Identity.Constants.ConfigurationSections;

namespace NetTemplate.Shared.WebApi.Identity.Extensions
{
    public static class ConfigurationExtensions
    {
        public static JwtConfig GetJwtConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(IdentityConfigurationSections.Jwt).Get<JwtConfig>();

        public static ClientsConfig GetClientsConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(IdentityConfigurationSections.Clients).Get<ClientsConfig>();
    }
}
