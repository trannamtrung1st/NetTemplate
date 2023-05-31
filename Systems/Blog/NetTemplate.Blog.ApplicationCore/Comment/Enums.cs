using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetTemplate.Blog.ApplicationCore.Comment
{
    public static class Enums
    {
        public enum CommentSortBy
        {
            [Column("CreatedTime")]
            [Description("Created time")]
            CreatedTime = 1,
        }
    }
}
