using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetTemplate.Blog.ApplicationCore.Post.Events;
using NetTemplate.Blog.ApplicationCore.Post.Views;
using NetTemplate.Shared.ApplicationCore.MemoryStore.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.Post
{
    public interface IPostViewManager
    {
        bool IsAvailable { get; }

        Task Initialize();
        Task RebuildAllViews();

        Task UpdateViewsOnEvent(PostCreatedEvent @event);
        Task UpdateViewsOnEvent(PostUpdatedEvent @event);
        Task UpdateViewsOnEvent(PostDeletedEvent @event);
        Task RebuildPostViews();
        Task<IEnumerable<PostView>> GetPostViews();
    }

    public class PostViewManager : IPostViewManager
    {
        private static bool _isAvailable;

        private readonly IMemoryStore _memoryStore;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostViewManager(
            IMemoryStore memoryStore,
            IPostRepository postRepository,
            IMapper mapper)
        {
            _isAvailable = false;

            _memoryStore = memoryStore;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public bool IsAvailable => _isAvailable;

        public async Task<IEnumerable<PostView>> GetPostViews()
        {
            ThrowIfNotAvailable();

            PostView[] views = await _memoryStore.HashGetAll<PostView>(Constants.CacheKey.PostView);

            return views;
        }

        public async Task Initialize()
        {
            if (_isAvailable) throw new InvalidOperationException();

            await Initialize(Constants.CacheKey.PostView, RebuildPostViews);

            _isAvailable = true;
        }

        public async Task RebuildAllViews()
        {
            await RebuildPostViews();
        }

        public async Task RebuildPostViews()
        {
            _isAvailable = false;

            IQueryable<PostEntity> query = _postRepository.GetQuery();

            PostView[] views = await _mapper.ProjectTo<PostView>(query).ToArrayAsync();

            string setKey = Constants.CacheKey.PostView;

            await _memoryStore.RemoveHash(setKey);

            await _memoryStore.HashSet(setKey,
                itemKeys: views.Select(v => v.Id.ToString()).ToArray(),
                items: views);

            _isAvailable = true;
        }

        private async Task Initialize(string cacheKey, Func<Task> action)
        {
            bool exists = await _memoryStore.KeyExists(cacheKey);

            if (!exists)
            {
                await action();
            }
        }

        public async Task UpdateViewsOnEvent(PostCreatedEvent @event)
        {
            ThrowIfNotAvailable();

            PostView view = await ConstructPostViewById(@event.Entity.Id);

            await _memoryStore.HashSet(Constants.CacheKey.PostView, view.Id.ToString(), view);
        }

        public async Task UpdateViewsOnEvent(PostUpdatedEvent @event)
        {
            ThrowIfNotAvailable();

            PostView view = await ConstructPostViewById(@event.EntityId);

            await _memoryStore.HashSet(Constants.CacheKey.PostView, view.Id.ToString(), view);
        }

        public async Task UpdateViewsOnEvent(PostDeletedEvent @event)
        {
            ThrowIfNotAvailable();

            await _memoryStore.HashRemove(Constants.CacheKey.PostView, @event.EntityId.ToString());
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
            if (!_isAvailable) throw new InvalidOperationException();
        }

        static class Constants
        {
            public static class CacheKey
            {
                public const string PostView = $"{nameof(PostViewManager)}_{nameof(PostView)}";
            }
        }
    }
}
