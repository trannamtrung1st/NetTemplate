using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.Post
{
    public interface IPostRepository : IRepository<PostEntity>
    {
        Task<QueryResponseModel<TResult>> Query<TResult>(
            string terms = null,
            IEnumerable<int> ids = null,
            int? categoryId = null,
            Enums.PostSortBy[] sortBy = null,
            bool[] isDesc = null,
            IOffsetPagingQuery paging = null,
            bool count = true,
            CancellationToken cancellationToken = default);

        Task<int> CountByCategory(int id, CancellationToken cancellationToken = default);
        Task<TResult> GetLatestPostOfCategory<TResult>(int id, CancellationToken cancellationToken = default);
    }
}
