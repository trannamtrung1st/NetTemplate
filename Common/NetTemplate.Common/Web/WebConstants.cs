namespace NetTemplate.Common.Web
{
    public static class WebConstants
    {
        public static class Jwt
        {
            public const string BearerTokenType = "Bearer";
        }

        public static class CommonHeaders
        {
            public const string IfNoneMatch = "If-None-Match";
            public const string Accept = "Accept";

            public static class CacheControl
            {
                public const string Name = "Cache-Control";
                public const string NoCache = "no-cache";
                public const string NoStore = "no-store";
            }
        }
    }
}
