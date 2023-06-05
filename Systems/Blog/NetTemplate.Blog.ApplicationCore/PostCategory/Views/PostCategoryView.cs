using NetTemplate.Blog.ApplicationCore.User.Views;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Views
{
    public class PostCategoryView : IHasId<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CreatorId { get; set; }
        public UserView Creator { get; set; }
        public int? LastModifyUserId { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset? LastModifiedTime { get; set; }
    }
}
