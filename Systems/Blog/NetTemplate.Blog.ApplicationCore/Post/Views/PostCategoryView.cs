using NetTemplate.Shared.ApplicationCore.Common.Views;

namespace NetTemplate.Blog.ApplicationCore.Post.Views
{
    public class PostCategoryView : VersionedView
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
