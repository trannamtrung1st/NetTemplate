using NetTemplate.Blog.ApplicationCore.Post.Views;

namespace NetTemplate.Blog.ApplicationCore.Post.Interfaces
{
    public interface IPostCache
    {
        Task<PostView> GetEntryOrAdd(int id, string currentVersion, Func<CancellationToken, Task<PostView>> createFunc, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
        Task<bool> UpdateEntry(PostView entry, string currentVersion, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
        Task<bool> RemoveEntry(int id, CancellationToken cancellationToken = default);
    }
}
