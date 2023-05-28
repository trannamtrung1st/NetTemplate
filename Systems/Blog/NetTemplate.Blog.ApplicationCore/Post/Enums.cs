using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetTemplate.Blog.ApplicationCore.Post
{
    public static class Enums
    {
        public enum PostSortBy
        {
            [Description("Title")]
            Title = 1,

            [Column("Category.Name")]
            [Description("Category name")]
            CategoryName = 2,

            [Column("Creator.UserCode")]
            [Description("Creator code")]
            CreatorCode = 3,
        }
    }
}
