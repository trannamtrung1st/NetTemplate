using NetTemplate.Blog.ApplicationCore.Post.Events;
using NetTemplate.Blog.ApplicationCore.Post.Views;

namespace NetTemplate.Blog.ApplicationCore.Post
{
    public interface IPostViewManager
    {
        bool IsReady { get; }

        Task Initialize();
        Task RebuildAllViews();

        Task HandleEvent(PostCreatedEvent @event);
        Task HandleEvent(PostUpdatedEvent @event);
        Task HandleEvent(PostDeletedEvent @event);
        Task RebuildPostViews();
        Task<IEnumerable<PostView>> GetPostViews();
    }
}
