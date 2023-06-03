using Microsoft.Extensions.Options;
using NetTemplate.Blog.ApplicationCore.Common.Models;
using NetTemplate.Blog.ApplicationCore.Post.Interfaces;
using NetTemplate.Blog.ApplicationCore.Post.Views;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Common.MemoryStore.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Implementations;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.Post.Implementations
{
    [ScopedService]
    public class PostViewManager : BaseViewManager, IPostViewManager
    {
        private readonly IPostRepository _postRepository;
        private readonly IEntityCache<PostView> _postCache;
        private readonly IOptions<ViewsConfig> _viewsOptions;

        public PostViewManager(
            IMemoryStore memoryStore,
            IPostRepository postRepository,
            IEntityCache<PostView> postCache,
            IOptions<ViewsConfig> viewsOptions) : base(memoryStore)
        {
            _postRepository = postRepository;
            _postCache = postCache;
            _viewsOptions = viewsOptions;
        }

        public async Task<PostView> GetPostView(int id, CancellationToken cancellationToken = default)
        {
            PostView view = await _postCache.GetEntryOrAdd(
                id.ToString(), _viewsOptions.Value.PostViewVersion,
                (cancellationToken) => ConstructPostViewById(id, cancellationToken), cancellationToken: cancellationToken);

            return view;
        }

        private async Task<PostView> ConstructPostViewById(int id, CancellationToken cancellationToken = default)
        {
            IQueryable<PostView> query = await _postRepository.QueryById<PostView>(id, cancellationToken);

            PostView view = query.FirstOrDefault();

            return view;
        }

        private static class Constants
        {
        }
    }
}
