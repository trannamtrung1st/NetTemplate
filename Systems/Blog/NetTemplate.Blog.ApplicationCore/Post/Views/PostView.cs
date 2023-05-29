using NetTemplate.Blog.ApplicationCore.User.Views;
using NetTemplate.Shared.ApplicationCore.Common.Entities;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Views
{
    public class PostView : VersionedModel, IHasId<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
        public virtual IEnumerable<string> Tags { get; set; }
        public virtual UserView Creator { get; set; }
        public virtual PostCategoryView Category { get; set; }
    }
}
