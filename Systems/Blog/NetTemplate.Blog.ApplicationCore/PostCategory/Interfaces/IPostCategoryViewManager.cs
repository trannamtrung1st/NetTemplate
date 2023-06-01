using NetTemplate.Blog.ApplicationCore.PostCategory.Events;
using NetTemplate.Blog.ApplicationCore.PostCategory.Views;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Interfaces
{
    // [NOTE] NoSQL/Memory store style
    public interface IPostCategoryViewManager
    {
        Task Initialize(CancellationToken cancellationToken = default);
        Task RebuildAllViews(CancellationToken cancellationToken = default);

        Task UpdateViewsOnEvent(PostCategoryCreatedEvent @event, CancellationToken cancellationToken = default);
        Task UpdateViewsOnEvent(PostCategoryUpdatedEvent @event, CancellationToken cancellationToken = default);
        Task UpdateViewsOnEvent(PostCategoryDeletedEvent @event, CancellationToken cancellationToken = default);

        bool IsPostCategoryAvailable { get; }
        Task RebuildPostCategoryViews(CancellationToken cancellationToken = default);
        Task<ListResponseModel<PostCategoryView>> FilterPostCategoryViews(
            string terms = null,
            IEnumerable<int> ids = null,
            Enums.PostCategorySortBy[] sortBy = null,
            bool[] isDesc = null,
            IOffsetPagingQuery paging = null,
            CancellationToken cancellationToken = default);
        Task<PostCategoryView> GetPostCategoryView(int id, CancellationToken cancellationToken = default);
    }
}
