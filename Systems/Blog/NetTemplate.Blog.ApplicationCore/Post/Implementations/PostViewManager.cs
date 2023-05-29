using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTemplate.Blog.ApplicationCore.Common.Models;
using NetTemplate.Blog.ApplicationCore.Post.Events;
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

        public async Task UpdateViewsOnEvent(PostDeletedEvent @event)
        {
            await _postCache.RemoveEntry(@event.EntityId);
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
            IQueryable<PostEntity> query = _postRepository.GetQuery().ById(id);

            PostView view = await _mapper.ProjectTo<PostView>(query).FirstOrDefaultAsync();

            return view;
        }

        private static class Constants
        {
        }
    }
}
