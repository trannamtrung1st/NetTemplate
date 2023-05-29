using NetTemplate.Blog.ApplicationCore.Post.Events;
using NetTemplate.Blog.ApplicationCore.Post.Views;

namespace NetTemplate.Blog.ApplicationCore.Post.Interfaces
{
    // [TODO] cache style
    public interface IPostViewManager
    {
        Task UpdateViewsOnEvent(PostDeletedEvent @event);

        Task<PostView> GetPostView(int id);
    }
}
