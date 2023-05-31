using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTemplate.Blog.ApplicationCore.Common.Models;
using NetTemplate.Blog.ApplicationCore.PostCategory.Events;
using NetTemplate.Blog.ApplicationCore.PostCategory.Interfaces;
using NetTemplate.Blog.ApplicationCore.PostCategory.Views;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Common.MemoryStore.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Implementations;
using NetTemplate.Shared.ApplicationCore.Common.Models;
using ViewPreservedKeys = NetTemplate.Shared.ApplicationCore.Common.Constants.ViewPreservedKeys;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Implementations
{
    [ScopedService]
    public class PostCategoryViewManager : BaseViewManager, IPostCategoryViewManager
    {
        private static bool _isPostCategoryAvailable;

        static PostCategoryViewManager()
        {
            _isPostCategoryAvailable = false;
        }

        // [NOTE] can be a NoSQL data store instead of memory store
        private readonly IMemoryStore _memoryStore;
        private readonly IOptions<ViewsConfig> _viewsOptions;
        private readonly IPostCategoryRepository _postCategoryRepository;
        private readonly IMapper _mapper;

        public PostCategoryViewManager(
            IMemoryStore memoryStore,
            IOptions<ViewsConfig> viewsOptions,
            IPostCategoryRepository postCategoryRepository,
            IMapper mapper) : base(memoryStore)
        {
            _memoryStore = memoryStore;
            _viewsOptions = viewsOptions;
            _postCategoryRepository = postCategoryRepository;
            _mapper = mapper;
        }

        public bool IsPostCategoryAvailable => _isPostCategoryAvailable;

        public async Task<ListResponseModel<PostCategoryView>> FilterPostCategoryViews(
            string terms = null,
            IEnumerable<int> ids = null,
            Enums.PostCategorySortBy[] sortBy = null,
            bool[] isDesc = null,
            IPagingQuery paging = null)
        {
            ThrowIfPostCategoryNotAvailable();

            PostCategoryView[] views = await _memoryStore.HashGetAll<PostCategoryView>(
                key: Constants.CacheKeys.PostCategoryView,
                exceptKeys: ViewPreservedKeys.All);

            IQueryable<PostCategoryView> query = views.AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(terms))
            {
                query = query.Where(e => e.Name.Contains(terms, StringComparison.OrdinalIgnoreCase));
            }

            query = query.ByIdsIfAny(ids);

            // Counting
            int total = await query.CountAsync();

            // Sorting
            query = query.SortBy(sortBy, isDesc);

            // Paging
            query = query.Paging(paging);

            return new ListResponseModel<PostCategoryView>(total, query);
        }

        public async Task<PostCategoryView> GetPostCategoryView(int id)
        {
            ThrowIfPostCategoryNotAvailable();

            PostCategoryView view = await _memoryStore.HashGet<PostCategoryView>(
                key: Constants.CacheKeys.PostCategoryView,
                itemKey: id.ToString());

            return view;
        }

        public async Task Initialize()
        {
            await Initialize(Constants.CacheKeys.PostCategoryView, _viewsOptions.Value.PostCategoryViewVersion, RebuildPostCategoryViews);
        }

        public async Task RebuildAllViews()
        {
            await RebuildPostCategoryViews();
        }

        public async Task RebuildPostCategoryViews()
        {
            _isPostCategoryAvailable = false;

            IQueryable<PostCategoryEntity> query = _postCategoryRepository.GetQuery();

            PostCategoryView[] views = await _mapper.ProjectTo<PostCategoryView>(query).ToArrayAsync();

            string setKey = Constants.CacheKeys.PostCategoryView;

            await _memoryStore.RemoveKey(setKey);

            await _memoryStore.HashSet(setKey,
                itemKeys: views.Select(v => v.Id.ToString()).ToArray(),
                items: views);

            _isPostCategoryAvailable = true;
        }

        public async Task UpdateViewsOnEvent(PostCategoryCreatedEvent @event)
        {
            ThrowIfPostCategoryNotAvailable();

            PostCategoryView view = await ConstructPostCategoryViewById(@event.Entity.Id);

            await _memoryStore.HashSet(Constants.CacheKeys.PostCategoryView, view.Id.ToString(), view);
        }

        public async Task UpdateViewsOnEvent(PostCategoryUpdatedEvent @event)
        {
            ThrowIfPostCategoryNotAvailable();

            PostCategoryView view = await ConstructPostCategoryViewById(@event.EntityId);

            await _memoryStore.HashSet(Constants.CacheKeys.PostCategoryView, view.Id.ToString(), view);
        }

        public async Task UpdateViewsOnEvent(PostCategoryDeletedEvent @event)
        {
            ThrowIfPostCategoryNotAvailable();

            await _memoryStore.HashRemove(Constants.CacheKeys.PostCategoryView, @event.EntityId.ToString());
        }

        private async Task<PostCategoryView> ConstructPostCategoryViewById(int id)
        {
            IQueryable<PostCategoryEntity> query = _postCategoryRepository.GetQuery()
                .Where(e => e.Id == id);

            PostCategoryView view = await _mapper.ProjectTo<PostCategoryView>(query).FirstOrDefaultAsync();

            return view;
        }

        private void ThrowIfPostCategoryNotAvailable()
        {
            if (!_isPostCategoryAvailable) throw new InvalidOperationException();
        }

        private static class Constants
        {
            public static class CacheKeys
            {
                public const string PostCategoryView = $"{nameof(PostCategoryViewManager)}_{nameof(PostCategoryView)}";
            }
        }
    }
}
