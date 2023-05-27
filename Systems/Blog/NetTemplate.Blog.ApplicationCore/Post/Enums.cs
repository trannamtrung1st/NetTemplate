using System.ComponentModel;

namespace NetTemplate.Blog.ApplicationCore.Post
{
    public static class Enums
    {
        public enum PostSortBy
        {
            [Description("Title")]
            Title = 1,

            [Description("Category name")]
            CategoryName = 2
        }
    }
}
