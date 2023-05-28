using NetTemplate.Blog.ApplicationCore.User.Views;
using NetTemplate.Shared.ApplicationCore.Common.Entities;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Views
{
    public class PostCategoryView : IHasId<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CreatorId { get; set; }
        public UserView Creator { get; set; }
    }
}
