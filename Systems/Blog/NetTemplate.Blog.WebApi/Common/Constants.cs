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
                public const string GetPostCategoryDetails = "{id}";
                public const string CreatePostCategory = "";
                public const string UpdatePostCategory = "{id}";
                public const string DeletePostCategory = "{id}";
            }

            public static class Post
            {
                public const string Base = "api/posts";
                public const string GetPosts = "";
                public const string GetPostDetails = "{id}";
                public const string CreatePost = "";
                public const string UpdatePost = "{id}";
                public const string UpdatePostTags = "{id}/tags";
                public const string DeletePost = "{id}";
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
