using NetTemplate.Common.Logging.Options;

namespace NetTemplate.Shared.WebApi.Logging
{
    public static class Constants
    {
        public static class ConfigurationSections
        {
            public const string RequestLogging = nameof(Serilog) + ":" + nameof(RequestLoggingOptions);
        }
    }
}
