using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.PostCategory
{
    public interface IPostCategoryRepository : IRepository<PostCategoryEntity>
    {
        Task<QueryResponseModel<PostCategoryEntity>> Query(
            string terms = null,
            IEnumerable<int> ids = null,
            Enums.PostCategorySortBy[] sortBy = null,
            bool[] isDesc = null,
            IPagingQuery paging = null,
            bool count = true);
    }
}
