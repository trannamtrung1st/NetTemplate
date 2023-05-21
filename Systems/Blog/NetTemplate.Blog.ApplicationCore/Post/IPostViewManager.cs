using NetTemplate.Blog.ApplicationCore.Post.Views;

namespace NetTemplate.Blog.ApplicationCore.Post
{
    public interface IPostViewManager
    {
        bool IsReady { get; }

        Task RebuildViews();
        Task<IEnumerable<PostView>> GetPostViews();
    }
}
