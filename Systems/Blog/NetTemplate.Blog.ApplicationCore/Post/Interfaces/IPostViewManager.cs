using NetTemplate.Blog.ApplicationCore.Post.Views;

namespace NetTemplate.Blog.ApplicationCore.Post.Interfaces
{
    // [NOTE] Cache style
    public interface IPostViewManager
    {
        Task<PostView> GetPostView(int id);
    }
}
