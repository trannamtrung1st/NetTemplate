using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTemplate.Blog.ApplicationCore.Common.Models;
using NetTemplate.Blog.ApplicationCore.Post.Events;
using NetTemplate.Blog.ApplicationCore.Post.Views;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Common.MemoryStore.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Implementations;

namespace NetTemplate.Blog.ApplicationCore.Post
{
    public interface IPostViewManager
    {
        Task Initialize();
        Task RebuildAllViews();

        Task UpdateViewsOnEvent(PostCreatedEvent @event);
        Task UpdateViewsOnEvent(PostUpdatedEvent @event);
        Task UpdateViewsOnEvent(PostDeletedEvent @event);

        bool IsPostAvailable { get; }
        Task RebuildPostViews();
        Task<IEnumerable<PostView>> GetPostViews();
    }

    [ScopedService]
    public class PostViewManager : BaseViewManager, IPostViewManager
    {
        private static bool _isPostAvailable;

        static PostViewManager()
        {
            _isPostAvailable = false;
        }

        private readonly IMemoryStore _memoryStore;
        private readonly IOptions<ViewsConfig> _viewsOptions;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostViewManager(
            IMemoryStore memoryStore,
            IOptions<ViewsConfig> viewsOptions,
            IPostRepository postRepository,
            IMapper mapper) : base(memoryStore)
        {
            _memoryStore = memoryStore;
            _viewsOptions = viewsOptions;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public bool IsPostAvailable => _isPostAvailable;

        public async Task<IEnumerable<PostView>> GetPostViews()
        {
            ThrowIfNotAvailable();

            PostView[] views = await _memoryStore.HashGetAll<PostView>(Constants.CacheKeys.PostView);

            return views;
        }

        public async Task Initialize()
        {
            await Initialize(Constants.CacheKeys.PostView, _viewsOptions.Value.PostViewVersion, RebuildPostViews);
        }

        public async Task RebuildAllViews()
        {
            await RebuildPostViews();
        }

        public async Task RebuildPostViews()
        {
            _isPostAvailable = false;

            IQueryable<PostEntity> query = _postRepository.GetQuery();

            PostView[] views = await _mapper.ProjectTo<PostView>(query).ToArrayAsync();

            string setKey = Constants.CacheKeys.PostView;

            await _memoryStore.RemoveKey(setKey);

            await _memoryStore.HashSet(setKey,
                itemKeys: views.Select(v => v.Id.ToString()).ToArray(),
                items: views);

            _isPostAvailable = true;
        }

        public async Task UpdateViewsOnEvent(PostCreatedEvent @event)
        {
            ThrowIfNotAvailable();

            PostView view = await ConstructPostViewById(@event.Entity.Id);

            await _memoryStore.HashSet(Constants.CacheKeys.PostView, view.Id.ToString(), view);
        }

        public async Task UpdateViewsOnEvent(PostUpdatedEvent @event)
        {
            ThrowIfNotAvailable();

            PostView view = await ConstructPostViewById(@event.EntityId);

            await _memoryStore.HashSet(Constants.CacheKeys.PostView, view.Id.ToString(), view);
        }

        public async Task UpdateViewsOnEvent(PostDeletedEvent @event)
        {
            ThrowIfNotAvailable();

            await _memoryStore.HashRemove(Constants.CacheKeys.PostView, @event.EntityId.ToString());
        }

        private async Task<PostView> ConstructPostViewById(int id)
        {
            IQueryable<PostEntity> query = _postRepository.GetQuery()
                .Where(e => e.Id == id);

            PostView view = await _mapper.ProjectTo<PostView>(query).FirstOrDefaultAsync();

            return view;
        }

        private void ThrowIfNotAvailable()
        {
            if (!_isPostAvailable) throw new InvalidOperationException();
        }

        private static class Constants
        {
            public static class CacheKeys
            {
                public const string PostView = $"{nameof(PostViewManager)}_{nameof(PostView)}";
            }
        }
    }
}
