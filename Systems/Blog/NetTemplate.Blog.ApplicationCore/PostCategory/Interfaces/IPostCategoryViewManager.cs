using NetTemplate.Blog.ApplicationCore.PostCategory.Events;
using NetTemplate.Blog.ApplicationCore.PostCategory.Views;

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
        Task<IEnumerable<PostCategoryView>> GetPostCategoryViews();
        Task<PostCategoryView> GetPostCategoryView(int id);
    }
}
