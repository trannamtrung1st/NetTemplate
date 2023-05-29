using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Views
{
    public class PostCategoryView : VersionedModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
