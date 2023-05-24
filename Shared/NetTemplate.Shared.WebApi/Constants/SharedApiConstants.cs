using NetTemplate.Common.Logging.Options;

namespace NetTemplate.Shared.WebApi.Constants
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
            public static class ApacheKafka
            {
                public const string ApacheKafkaService = nameof(ApacheKafkaService);
                public const string Enabled = ApacheKafkaService + ":" + nameof(Enabled);
            }

            public static class Logging
            {
                public const string RequestLogging = nameof(Serilog) + ":" + nameof(RequestLoggingOptions);
            }
        }

        public static class ConnectionStrings
        {
            public const string Master = nameof(Master);
        }

        public static class LogProperties
        {
            public const string UserCode = nameof(UserCode);
        }
    }
}
