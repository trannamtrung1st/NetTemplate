using Microsoft.Extensions.Options;
using NetTemplate.Blog.ApplicationCore.Common.Models;
using NetTemplate.Blog.ApplicationCore.Post.Interfaces;
using NetTemplate.Blog.ApplicationCore.Post.Views;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Common.MemoryStore.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Implementations;

namespace NetTemplate.Blog.ApplicationCore.Post.Implementations
{
    [ScopedService]
    public class PostViewManager : BaseViewManager, IPostViewManager
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostCache _postCache;
        private readonly IOptions<ViewsConfig> _viewsOptions;

        public PostViewManager(
            IMemoryStore memoryStore,
            IPostRepository postRepository,
            IPostCache postCache,
            IOptions<ViewsConfig> viewsOptions) : base(memoryStore)
        {
            _postRepository = postRepository;
            _postCache = postCache;
            _viewsOptions = viewsOptions;
        }

        public async Task<PostView> GetPostView(int id)
        {
            PostView view = await _postCache.GetEntryOrAdd(
                id, _viewsOptions.Value.PostViewVersion,
                () => ConstructPostViewById(id));

            return view;
        }

        private async Task<PostView> ConstructPostViewById(int id)
        {
            IQueryable<PostView> query = await _postRepository.QueryById<PostView>(id);

            PostView view = query.FirstOrDefault();

            return view;
        }

        private static class Constants
        {
        }
    }
}
