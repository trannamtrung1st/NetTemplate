using NetTemplate.Common.Logging.Options;
using NetTemplate.Shared.WebApi.Identity.Models;

namespace NetTemplate.Shared.WebApi.Common.Constants
{
    public static class SharedApiConstants
    {
        public static class Environment
        {
            public const string VariableName = "ASPNETCORE_ENVIRONMENT";
            public const string Development = nameof(Development);
            public const string Production = nameof(Production);
        }

        public static class SwaggerDefaults
        {
            public const string Prefix = "swagger";
            public const string DocEndpointFormat = "/swagger/{0}/swagger.json";
        }

        public static class Versioning
        {
            public const string GroupNameFormat = "'v'VVV";
        }

        public static class ConfigKeys
        {
            public static class Logging
            {
                public const string RequestLogging = nameof(Serilog) + ":" + nameof(RequestLoggingOptions);
            }

            public static class Identity
            {
                public const string DefaultJwtSection = nameof(JwtConfig);
                public const string DefaultClientsConfig = nameof(ClientsConfig);
            }
        }

        public static class LogProperties
        {
            public const string UserCode = nameof(UserCode);
        }
    }
}
