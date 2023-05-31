using NetTemplate.Blog.ApplicationCore.PostCategory.Events;
using NetTemplate.Blog.ApplicationCore.PostCategory.Views;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Interfaces
{
    // [NOTE] NoSQL/Memory store style
    public interface IPostCategoryViewManager
    {
        Task Initialize();
        Task RebuildAllViews();

        Task UpdateViewsOnEvent(PostCategoryCreatedEvent @event);
        Task UpdateViewsOnEvent(PostCategoryUpdatedEvent @event);
        Task UpdateViewsOnEvent(PostCategoryDeletedEvent @event);

        bool IsPostCategoryAvailable { get; }
        Task RebuildPostCategoryViews();
        Task<ListResponseModel<PostCategoryView>> FilterPostCategoryViews(
            string terms = null,
            IEnumerable<int> ids = null,
            Enums.PostCategorySortBy[] sortBy = null,
            bool[] isDesc = null,
            IOffsetPagingQuery paging = null);
        Task<PostCategoryView> GetPostCategoryView(int id);
    }
}
