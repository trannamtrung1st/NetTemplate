using AutoMapper;
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
        private readonly IMapper _mapper;

        public PostViewManager(
            IMemoryStore memoryStore,
            IPostRepository postRepository,
            IPostCache postCache,
            IOptions<ViewsConfig> viewsOptions,
            IMapper mapper) : base(memoryStore)
        {
            _postRepository = postRepository;
            _postCache = postCache;
            _viewsOptions = viewsOptions;
            _mapper = mapper;
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
            IQueryable<PostEntity> query = await _postRepository.QueryById(id);

            PostView view = _mapper.ProjectTo<PostView>(query).FirstOrDefault();

            return view;
        }

        private static class Constants
        {
        }
    }
}
