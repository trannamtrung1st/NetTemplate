using NetTemplate.Shared.Infrastructure.Background.Models;

namespace NetTemplate.Shared.Infrastructure.Background
{
    // [TODO] refactor constants
    public static class BackgroundConstants
    {
        public static class DefaultConnectionNames
        {
            public const string Hangfire = nameof(Hangfire);
            public const string Master = "Master";
        }

        public static class Defaults
        {
            public const int DefaultRetryAttempts = 3;
            public const int DefaultSecondsFactor = 2;
        }

        public const string DefaultConfigSection = nameof(HangfireConfig);
    }
}
