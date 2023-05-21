using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetTemplate.Blog.ApplicationCore.Post;
using NetTemplate.Blog.ApplicationCore.Post.Events;
using NetTemplate.Blog.ApplicationCore.Post.Views;
using NetTemplate.Blog.Infrastructure.MemoryStore.Interfaces;

namespace NetTemplate.Blog.Infrastructure.Domains.Post
{
    public class PostViewManager : IPostViewManager
    {
        private bool _isReady;

        private readonly IMemoryStore _memoryStore;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostViewManager(
            IMemoryStore memoryStore,
            IPostRepository postRepository,
            IMapper mapper)
        {
            _isReady = false;

            _memoryStore = memoryStore;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public bool IsReady => _isReady;

        public async Task<IEnumerable<PostView>> GetPostViews()
        {
            ThrowIfNotReady();

            PostView[] views = await _memoryStore.HashGetAll<PostView>(Constants.CacheKey.PostView);

            return views;
        }

        public async Task Initialize()
        {
            if (_isReady) throw new InvalidOperationException();

            await Initialize(Constants.CacheKey.PostView, RebuildPostViews);

            _isReady = true;
        }

        public async Task RebuildAllViews()
        {
            await RebuildPostViews();
        }

        public async Task RebuildPostViews()
        {
            IQueryable<PostEntity> query = _postRepository.GetQuery();

            PostView[] views = await _mapper.ProjectTo<PostView>(query).ToArrayAsync();

            string setKey = Constants.CacheKey.PostView;

            await _memoryStore.RemoveHash(setKey);

            await _memoryStore.HashSet(setKey,
                itemKeys: views.Select(v => v.Id.ToString()).ToArray(),
                items: views);
        }

        private async Task Initialize(string cacheKey, Func<Task> action)
        {
            bool exists = await _memoryStore.KeyExists(cacheKey);

            if (!exists)
            {
                await action();
            }
        }

        public async Task HandleEvent(PostCreatedEvent @event)
        {
            ThrowIfNotReady();

            PostView view = await ConstructPostViewById(@event.Entity.Id);

            await _memoryStore.HashSet(Constants.CacheKey.PostView, view.Id.ToString(), view);
        }

        public async Task HandleEvent(PostUpdatedEvent @event)
        {
            ThrowIfNotReady();

            PostView view = await ConstructPostViewById(@event.EntityId);

            await _memoryStore.HashSet(Constants.CacheKey.PostView, view.Id.ToString(), view);
        }

        public async Task HandleEvent(PostDeletedEvent @event)
        {
            ThrowIfNotReady();

            await _memoryStore.HashRemove(Constants.CacheKey.PostView, @event.EntityId.ToString());
        }

        private async Task<PostView> ConstructPostViewById(int id)
        {
            IQueryable<PostEntity> query = _postRepository.GetQuery()
                .Where(e => e.Id == id);

            PostView view = await _mapper.ProjectTo<PostView>(query).FirstOrDefaultAsync();

            return view;
        }

        private void ThrowIfNotReady()
        {
            if (!_isReady) throw new InvalidOperationException();
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
