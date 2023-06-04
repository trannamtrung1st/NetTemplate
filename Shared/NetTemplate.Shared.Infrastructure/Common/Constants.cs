using NetTemplate.Shared.ClientSDK.Common.Models;

namespace NetTemplate.Shared.Infrastructure.Common
{
    public static class Constants
    {
        public static class DefaultPaths
        {
            public const string DevelopmentSharedDirectory = "../Shared/";
            public const string AppsettingsPath = "appsettings.json";
            public const string AppsettingsSharedPath = "appsettings.shared.json";
        }

        public static class ConfigurationSections
        {
            public const string ClientSDK = nameof(ClientConfig);
        }
    }
}
