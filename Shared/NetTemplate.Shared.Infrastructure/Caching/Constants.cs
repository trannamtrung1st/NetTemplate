namespace NetTemplate.Shared.Infrastructure.Caching
{
    public static class Constants
    {
        public static class CachingProviders
        {
            public const string InMemory = nameof(InMemory);
            public const string Redis = nameof(Redis);
        }
    }
}
