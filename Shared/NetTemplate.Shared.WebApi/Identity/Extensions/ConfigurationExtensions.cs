using NetTemplate.Shared.WebApi.Identity.Models;
using ConfigurationSections = NetTemplate.Shared.WebApi.Identity.Constants.ConfigurationSections;

namespace NetTemplate.Shared.WebApi.Identity.Extensions
{
    public static class ConfigurationExtensions
    {
        public static JwtConfig GetJwtConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(ConfigurationSections.Jwt).Get<JwtConfig>();

        public static ClientsConfig GetClientsConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(ConfigurationSections.Clients).Get<ClientsConfig>();

        public static SimulatedAuthConfig GetSimulatedAuthConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(ConfigurationSections.SimulatedAuth).Get<SimulatedAuthConfig>();
    }
}
