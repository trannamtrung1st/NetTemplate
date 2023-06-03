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

        public PostCategoryViewManager(
            IMemoryStore memoryStore,
            IOptions<ViewsConfig> viewsOptions,
            IPostCategoryRepository postCategoryRepository) : base(memoryStore)
        {
            _memoryStore = memoryStore;
            _viewsOptions = viewsOptions;
            _postCategoryRepository = postCategoryRepository;
        }

        public bool IsPostCategoryAvailable => _isPostCategoryAvailable;

        public async Task<ListResponseModel<PostCategoryView>> FilterPostCategoryViews(
            string terms = null,
            IEnumerable<int> ids = null,
            Enums.PostCategorySortBy[] sortBy = null,
            bool[] isDesc = null,
            IOffsetPagingQuery paging = null,
            CancellationToken cancellationToken = default)
        {
            ThrowIfPostCategoryNotAvailable();

            PostCategoryView[] views = await _memoryStore.HashGetAll<PostCategoryView>(
                key: Constants.CacheKeys.PostCategoryView,
                exceptKeys: ViewPreservedKeys.All,
                cancellationToken);

            IQueryable<PostCategoryView> query = views.AsQueryable();

            if (!string.IsNullOrEmpty(terms))
            {
                query = query.Where(e => e.Name.Contains(terms, StringComparison.OrdinalIgnoreCase));
            }

            query = query.ByIdsIfAny(ids);

            int total = query.Count();

            query = query.SortBy(sortBy, isDesc);

            query = query.OffsetPaging(paging);

            return new ListResponseModel<PostCategoryView>(total, query);
        }

        public async Task<PostCategoryView> GetPostCategoryView(int id, CancellationToken cancellationToken = default)
        {
            ThrowIfPostCategoryNotAvailable();

            PostCategoryView view = await _memoryStore.HashGet<PostCategoryView>(
                key: Constants.CacheKeys.PostCategoryView,
                itemKey: id.ToString(),
                cancellationToken);

            return view;
        }

        public async Task Initialize(CancellationToken cancellationToken = default)
        {
            await Initialize(Constants.CacheKeys.PostCategoryView,
                _viewsOptions.Value.PostCategoryViewVersion,
                RebuildPostCategoryViews,
                (_) =>
                {
                    _isPostCategoryAvailable = true;
                    return Task.CompletedTask;
                },
                cancellationToken);
        }

        public async Task RebuildAllViews(CancellationToken cancellationToken = default)
        {
            await RebuildPostCategoryViews(cancellationToken);
        }

        public async Task RebuildPostCategoryViews(CancellationToken cancellationToken = default)
        {
            _isPostCategoryAvailable = false;

            IQueryable<PostCategoryView> query = await _postCategoryRepository.QueryAll<PostCategoryView>(cancellationToken);

            PostCategoryView[] views = query.ToArray();

            string setKey = Constants.CacheKeys.PostCategoryView;

            await _memoryStore.RemoveKey(setKey, cancellationToken);

            await _memoryStore.HashSet(setKey,
                itemKeys: views.Select(v => v.Id.ToString()).ToArray(),
                items: views,
                cancellationToken);

            _isPostCategoryAvailable = true;
        }

        public async Task UpdateViewsOnEvent(PostCategoryCreatedEvent @event, CancellationToken cancellationToken = default)
        {
            ThrowIfPostCategoryNotAvailable();

            PostCategoryView view = await ConstructPostCategoryViewById(@event.Entity.Id, cancellationToken);

            await _memoryStore.HashSet(Constants.CacheKeys.PostCategoryView, view.Id.ToString(), view, cancellationToken);
        }

        public async Task UpdateViewsOnEvent(PostCategoryUpdatedEvent @event, CancellationToken cancellationToken = default)
        {
            ThrowIfPostCategoryNotAvailable();

            PostCategoryView view = await ConstructPostCategoryViewById(@event.EntityId, cancellationToken);

            await _memoryStore.HashSet(Constants.CacheKeys.PostCategoryView, view.Id.ToString(), view, cancellationToken);
        }

        public async Task UpdateViewsOnEvent(PostCategoryDeletedEvent @event, CancellationToken cancellationToken = default)
        {
            ThrowIfPostCategoryNotAvailable();

            await _memoryStore.HashRemove(Constants.CacheKeys.PostCategoryView, @event.EntityId.ToString(), cancellationToken);
        }

        private async Task<PostCategoryView> ConstructPostCategoryViewById(int id, CancellationToken cancellationToken = default)
        {
            IQueryable<PostCategoryView> query = await _postCategoryRepository.QueryById<PostCategoryView>(id, cancellationToken);

            PostCategoryView view = query.FirstOrDefault();

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
