﻿using Microsoft.EntityFrameworkCore;
using NetTemplate.Blog.ApplicationCore.Post;
using NetTemplate.Blog.Infrastructure.Persistence;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.ApplicationCore.Common.Models;
using NetTemplate.Shared.Infrastructure.Persistence.Repositories;

namespace NetTemplate.Blog.Infrastructure.Domains.Post
{
    [ScopedService]
    public class PostRepository : EFCoreRepository<PostEntity, MainDbContext>, IPostRepository
    {
        public PostRepository(MainDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> CountByCategory(int id)
        {
            int count = await DbSet.Where(e => e.CategoryId == id).CountAsync();

            return count;
        }

        // [TODO] add cancellation tokens
        public override async Task<PostEntity> FindById(params object[] keys)
        {
            int id = GetIdFromKeys<int>(keys);

            PostEntity entity = await DbSet.ById(id)
                .Select(PostEntity.SelectBasicInfoExpression)
                .FirstOrDefaultAsync();

            if (entity != null)
            {
                await Track(entity);

                await LoadAggregate(entity);
            }

            return entity;
        }

        public Task<IQueryable<PostEntity>> GetLatestPostOfCategory(int id)
        {
            IOrderedQueryable<PostEntity> latestPostQuery = DbSet.Where(e => e.CategoryId == id)
                .OrderByDescending(e => e.CreatedTime);

            return Task.FromResult<IQueryable<PostEntity>>(latestPostQuery);
        }

        public async Task<QueryResponseModel<PostEntity>> Query(
            string terms = null,
            IEnumerable<int> ids = null,
            int? categoryId = null,
            Enums.PostSortBy[] sortBy = null,
            bool[] isDesc = null,
            IPagingQuery paging = null,
            bool count = true)
        {
            IQueryable<PostEntity> query = DbSet;

            if (!string.IsNullOrEmpty(terms))
            {
                query = query.Where(e => e.Title.Contains(terms));
            }

            query = query.ByIdsIfAny(ids);

            if (categoryId != null)
            {
                query = query.Where(e => e.Category.Id == categoryId);
            }

            int? total = null;

            if (count)
            {
                total = await query
                    .JoinRequiredRelationships()
                    .CountAsync();
            }

            query = query.SortBy(sortBy, isDesc);

            query = query.Paging(paging);

            return new QueryResponseModel<PostEntity>(total, query);
        }

        public override Task<IQueryable<PostEntity>> QueryById(params object[] keys)
            => QueryById<PostEntity, int>(keys);

        protected override async Task LoadAggregate(PostEntity entity)
        {
            await dbContext.Entry(entity).Collection(e => e.Tags).LoadAsync();
        }
    }
}
