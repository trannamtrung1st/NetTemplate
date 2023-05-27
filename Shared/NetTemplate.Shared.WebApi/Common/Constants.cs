using NetTemplate.Common.Logging.Options;
using NetTemplate.Shared.WebApi.Common.Models;
using NetTemplate.Shared.WebApi.Identity.Models;

namespace NetTemplate.Shared.WebApi.Common
{
    public static class Constants
    {
        public static class Environment
        {
            public const string VariableName = "ASPNETCORE_ENVIRONMENT";
            public const string Development = nameof(Development);
            public const string Production = nameof(Production);
        }

        public static class Swagger
        {
            public const string Prefix = "swagger";
            public const string DocEndpointFormat = "/swagger/{0}/swagger.json";
        }

        public static class Versioning
        {
            public const string GroupNameFormat = "'v'VVV";
        }

        public static class ConfigurationSections
        {
            public static class Common
            {
                public const string WebInfo = nameof(WebInfoConfig);
            }

            public static class Logging
            {
                public const string RequestLogging = nameof(Serilog) + ":" + nameof(RequestLoggingOptions);
            }

            public static class Identity
            {
                public const string Jwt = nameof(JwtConfig);
                public const string Clients = nameof(ClientsConfig);
            }
        }

        public static class LogProperties
        {
            public const string UserCode = nameof(UserCode);
        }

        public static class Messages
        {
            public static class Swagger
            {
                public const string Instruction = "Please go to /swagger for API documentation.";
            }
        }
    }
}
