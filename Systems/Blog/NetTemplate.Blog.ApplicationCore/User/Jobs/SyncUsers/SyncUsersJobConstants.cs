namespace NetTemplate.Blog.ApplicationCore.User.Jobs.SyncUsers
{
    public static class SyncUsersJobConstants
    {
        public const int TimeOutMs = 1 * 60 * 60 * 1000;
        public const int InitialDelay = 15;
        public const int DefaultAttempts = 3;
        public static readonly int[] DefaultDelaysInSeconds = new[]
        {
            InitialDelay,
            (int)(InitialDelay * 1.25),
            (int)(InitialDelay * 1.5)
        };
    }
}
