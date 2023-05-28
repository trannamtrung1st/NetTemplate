﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTemplate.Blog.ApplicationCore.Common.Models;
using NetTemplate.Blog.ApplicationCore.PostCategory.Events;
using NetTemplate.Blog.ApplicationCore.PostCategory.Views;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Common.MemoryStore.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Implementations;

namespace NetTemplate.Blog.ApplicationCore.PostCategory
{
    public interface IPostCategoryViewManager
    {
        Task Initialize();
        Task RebuildAllViews();

        Task UpdateViewsOnEvent(PostCategoryCreatedEvent @event);
        Task UpdateViewsOnEvent(PostCategoryUpdatedEvent @event);
        Task UpdateViewsOnEvent(PostCategoryDeletedEvent @event);

        bool IsPostCategoryAvailable { get; }
        Task RebuildPostCategoryViews();
        Task<IEnumerable<PostCategoryView>> GetPostCategoryViews();
    }

    [ScopedService]
    public class PostCategoryViewManager : BaseViewManager, IPostCategoryViewManager
    {
        private static bool _isPostCategoryAvailable;

        static PostCategoryViewManager()
        {
            _isPostCategoryAvailable = false;
        }

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

        public async Task<IEnumerable<PostCategoryView>> GetPostCategoryViews()
        {
            ThrowIfNotAvailable();

            PostCategoryView[] views = await _memoryStore.HashGetAll<PostCategoryView>(Constants.CacheKeys.PostCategoryView);

            return views;
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
            ThrowIfNotAvailable();

            PostCategoryView view = await ConstructPostCategoryViewById(@event.Entity.Id);

            await _memoryStore.HashSet(Constants.CacheKeys.PostCategoryView, view.Id.ToString(), view);
        }

        public async Task UpdateViewsOnEvent(PostCategoryUpdatedEvent @event)
        {
            ThrowIfNotAvailable();

            PostCategoryView view = await ConstructPostCategoryViewById(@event.EntityId);

            await _memoryStore.HashSet(Constants.CacheKeys.PostCategoryView, view.Id.ToString(), view);
        }

        public async Task UpdateViewsOnEvent(PostCategoryDeletedEvent @event)
        {
            ThrowIfNotAvailable();

            await _memoryStore.HashRemove(Constants.CacheKeys.PostCategoryView, @event.EntityId.ToString());
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
