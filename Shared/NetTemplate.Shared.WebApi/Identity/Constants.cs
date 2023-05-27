using NetTemplate.Shared.WebApi.Identity.Models;

namespace NetTemplate.Shared.WebApi.Identity
{
    public static class Constants
    {
        public static class ConfigurationSections
        {
            public const string Jwt = nameof(JwtConfig);
            public const string Clients = nameof(ClientsConfig);
            public const string SimulatedAuth = nameof(SimulatedAuthConfig);
        }
    }
}
