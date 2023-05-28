using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetTemplate.Blog.ApplicationCore.PostCategory
{
    public static class Enums
    {
        public enum PostCategorySortBy
        {
            [Description("Name")]
            Name = 1,

            [Column("Creator.FirstName")]
            [Description("Creator first name")]
            CreatorFirstName = 2,

            [Column("Creator.FullName")]
            [Description("Creator full name")]
            CreatorFullName = 3,

            [Column("Creator.UserCode")]
            [Description("Creator code")]
            CreatorCode = 4,
        }
    }
}
