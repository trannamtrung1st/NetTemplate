using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Models
{
    public class PostListRequestModel : BaseSortableFilterQuery<Enums.PostSortBy>
    {
        public int? CategoryId { get; set; }
        public IEnumerable<int> Ids { get; set; }

        public override bool CanGetAll() => false;
    }
}
