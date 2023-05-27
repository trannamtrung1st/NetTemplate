using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Models
{
    public class PostCategoryListRequestModel : BaseSortableFilterQuery<Enums.PostCategorySortBy>
    {
        public IEnumerable<int> Ids { get; set; }

        public override bool CanGetAll() => false;
    }
}
