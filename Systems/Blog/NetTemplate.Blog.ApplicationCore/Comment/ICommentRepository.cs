using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.Comment
{
    public interface ICommentRepository : IRepository<CommentEntity>
    {
        Task<QueryResponseModel<TResult>> Query<TResult>(int onPostId,
            IEnumerable<int> ids = null,
            int? creatorId = null,
            Enums.CommentSortBy[] sortBy = null,
            bool[] isDesc = null,
            IKeySetPagingQuery<DateTimeOffset> paging = null,
            bool count = true,
            CancellationToken cancellationToken = default);
    }
}
