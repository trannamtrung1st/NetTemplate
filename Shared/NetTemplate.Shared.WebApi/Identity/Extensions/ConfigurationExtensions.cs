using NetTemplate.Shared.WebApi.Identity.Models;

namespace NetTemplate.Shared.WebApi.Identity.Extensions
{
    public static class ConfigurationExtensions
    {
        public static JwtConfig GetJwtConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(JwtConfig.ConfigurationSection).Get<JwtConfig>();

        public static ApplicationClientsConfig GetClientsConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(ApplicationClientsConfig.ConfigurationSection).Get<ApplicationClientsConfig>();

        public static SimulatedAuthConfig GetSimulatedAuthConfigDefaults(this IConfiguration configuration)
            => configuration.GetSection(SimulatedAuthConfig.ConfigurationSection).Get<SimulatedAuthConfig>();
    }
}
