using NetTemplate.Blog.ApplicationCore.Post.Interfaces;
using NetTemplate.Blog.ApplicationCore.Post.Views;

namespace NetTemplate.Blog.Infrastructure.Domains.Post.Implementations
{
    // [TODO] redis
    public class RedisPostCache : IPostCache
    {
        public Task<PostView> GetEntryOrAdd(int id, string currentVersion, Func<CancellationToken, Task<PostView>> creatFunc, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveEntry(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateEntry(PostView entry, string currentVersion, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
