using NetTemplate.Blog.ApplicationCore.Post.Views;

namespace NetTemplate.Blog.ApplicationCore.Post.Interfaces
{
    public interface IPostCache
    {
        Task<PostView> GetEntryOrAdd(int id, string currentVersion, Func<Task<PostView>> createFunc, TimeSpan? expiration = null);
        Task<bool> UpdateEntry(PostView entry, string currentVersion, TimeSpan? expiration = null);
        Task<bool> RemoveEntry(int id);
    }
}
