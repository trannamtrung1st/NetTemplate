namespace NetTemplate.Shared.ClientSDK.Identity
{
    public static class Constants
    {
        public const string ClientName = "Identity";

        public static class ApiEndpoints
        {
            public static class User
            {
                public const string GetUsers = "/api/users";
            }
        }

        public static class ApiScopes
        {
            public const string Identity = "identity";
        }
    }
}
