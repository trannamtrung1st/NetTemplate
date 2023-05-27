using NetTemplate.Shared.WebApi.Common.Models;

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

        public static class Versioning
        {
            public const string GroupNameFormat = "'v'VVV";
        }

        public static class ConfigurationSections
        {
            public const string WebInfo = nameof(WebInfoConfig);
        }

        public static class LogProperties
        {
            public const string UserCode = nameof(UserCode);
        }

        public static class Messages
        {
            public const string SwaggerInstruction = "Please go to /swagger for API documentation.";
        }
    }
}
