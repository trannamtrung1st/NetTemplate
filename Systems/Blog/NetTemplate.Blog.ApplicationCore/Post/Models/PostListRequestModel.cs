using NetTemplate.Shared.ApplicationCore.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Models
{
    public class PostListRequestModel : BaseSortableFilterQuery<PostConstants.SortBy>
    {
        public int? CategoryId { get; set; }

        public override bool CanGetAll() => false;
    }
}
