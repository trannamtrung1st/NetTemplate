using NetTemplate.Shared.Infrastructure.Background.Models;

namespace NetTemplate.Shared.Infrastructure.Background
{
    public static class Constants
    {
        public static class ConnectionNames
        {
            public const string Hangfire = nameof(Hangfire);
            public const string Master = "Master";
        }

        public static class Configurations
        {
            public const int RetryAttempts = 3;
            public const int SecondsFactor = 2;
        }

        public const string ConfigurationSection = nameof(HangfireConfig);
    }
}
