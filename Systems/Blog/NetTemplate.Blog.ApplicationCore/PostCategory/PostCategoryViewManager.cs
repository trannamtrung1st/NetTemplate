using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetTemplate.Blog.ApplicationCore.PostCategory.Events;
using NetTemplate.Blog.ApplicationCore.PostCategory.Views;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Common.MemoryStore.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.PostCategory
{
    public interface IPostCategoryViewManager
    {
        bool IsAvailable { get; }

        Task Initialize();
        Task RebuildAllViews();

        Task UpdateViewsOnEvent(PostCategoryCreatedEvent @event);
        Task UpdateViewsOnEvent(PostCategoryUpdatedEvent @event);
        Task UpdateViewsOnEvent(PostCategoryDeletedEvent @event);
        Task RebuildPostCategoryViews();
        Task<IEnumerable<PostCategoryView>> GetPostCategoryViews();
    }

    [ScopedService]
    public class PostCategoryViewManager : IPostCategoryViewManager
    {
        private static bool _isAvailable;

        static PostCategoryViewManager()
        {
            _isAvailable = false;
        }

        private readonly IMemoryStore _memoryStore;
        private readonly IPostCategoryRepository _postCategoryRepository;
        private readonly IMapper _mapper;

        public PostCategoryViewManager(
            IMemoryStore memoryStore,
            IPostCategoryRepository postCategoryRepository,
            IMapper mapper)
        {
            _memoryStore = memoryStore;
            _postCategoryRepository = postCategoryRepository;
            _mapper = mapper;
        }

        public bool IsAvailable => _isAvailable;

        public async Task<IEnumerable<PostCategoryView>> GetPostCategoryViews()
        {
            ThrowIfNotAvailable();

            PostCategoryView[] views = await _memoryStore.HashGetAll<PostCategoryView>(Constants.CacheKey.PostCategoryView);

            return views;
        }

        public async Task Initialize()
        {
            if (_isAvailable) throw new InvalidOperationException();

            await Initialize(Constants.CacheKey.PostCategoryView, RebuildPostCategoryViews);

            _isAvailable = true;
        }

        public async Task RebuildAllViews()
        {
            await RebuildPostCategoryViews();
        }

        public async Task RebuildPostCategoryViews()
        {
            _isAvailable = false;

            IQueryable<PostCategoryEntity> query = _postCategoryRepository.GetQuery();

            PostCategoryView[] views = await _mapper.ProjectTo<PostCategoryView>(query).ToArrayAsync();

            string setKey = Constants.CacheKey.PostCategoryView;

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

        public async Task UpdateViewsOnEvent(PostCategoryCreatedEvent @event)
        {
            ThrowIfNotAvailable();

            PostCategoryView view = await ConstructPostCategoryViewById(@event.Entity.Id);

            await _memoryStore.HashSet(Constants.CacheKey.PostCategoryView, view.Id.ToString(), view);
        }

        public async Task UpdateViewsOnEvent(PostCategoryUpdatedEvent @event)
        {
            ThrowIfNotAvailable();

            PostCategoryView view = await ConstructPostCategoryViewById(@event.EntityId);

            await _memoryStore.HashSet(Constants.CacheKey.PostCategoryView, view.Id.ToString(), view);
        }

        public async Task UpdateViewsOnEvent(PostCategoryDeletedEvent @event)
        {
            ThrowIfNotAvailable();

            await _memoryStore.HashRemove(Constants.CacheKey.PostCategoryView, @event.EntityId.ToString());
        }

        private async Task<PostCategoryView> ConstructPostCategoryViewById(int id)
        {
            IQueryable<PostCategoryEntity> query = _postCategoryRepository.GetQuery()
                .Where(e => e.Id == id);

            PostCategoryView view = await _mapper.ProjectTo<PostCategoryView>(query).FirstOrDefaultAsync();

            return view;
        }

        private void ThrowIfNotAvailable()
        {
            if (!_isAvailable) throw new InvalidOperationException();
        }

        private static class Constants
        {
            public static class CacheKey
            {
                public const string PostCategoryView = $"{nameof(PostCategoryViewManager)}_{nameof(PostCategoryView)}";
            }
        }
    }
}
