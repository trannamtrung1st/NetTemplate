namespace NetTemplate.Blog.WebApi.Common
{
    public static class Constants
    {
        public static class ApiRoutes
        {
            public static class Home
            {
                public const string Base = "";
                public const string Welcome = "";
            }

            public static class Error
            {
                public const string Base = "error";
                public const string HandleException = "";
            }

            public static class PostCategory
            {
                public const string Base = "api/post-categories";
                public const string GetPostCategories = "";
            }
        }

        public static class CacheProfiles
        {
            public const string Sample = nameof(Sample);
        }

        public static class Messages
        {
            public const string ApiWelcome = "({0}) API {1} is running!";
        }
    }
}
