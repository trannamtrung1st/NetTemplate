using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        public PostRepository(MainDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<int> CountByCategory(int id, CancellationToken cancellationToken = default)
        {
            int count = await DbSet.Where(e => e.CategoryId == id).CountAsync(cancellationToken);

            return count;
        }

        public override async Task<PostEntity> FindById(params object[] keys)
        {
            int id = GetIdFromObject<int>(keys);

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

        public async Task<TResult> GetLatestPostOfCategory<TResult>(int id, CancellationToken cancellationToken = default)
        {
            IOrderedQueryable<PostEntity> latestPostQuery = DbSet.Where(e => e.CategoryId == id)
                .OrderByDescending(e => e.CreatedTime);

            TResult result = await mapper
                .CustomProjectTo<TResult>(latestPostQuery)
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }

        public async Task<QueryResponseModel<TResult>> Query<TResult>(
            string terms = null,
            IEnumerable<int> ids = null,
            int? categoryId = null,
            Enums.PostSortBy[] sortBy = null,
            bool[] isDesc = null,
            IOffsetPagingQuery paging = null,
            bool count = true,
            CancellationToken cancellationToken = default)
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
                    .CountAsync(cancellationToken);
            }

            query = query.SortBy(sortBy, isDesc);

            query = query.OffsetPaging(paging);

            IQueryable<TResult> result = mapper.CustomProjectTo<TResult>(query);

            return new QueryResponseModel<TResult>(total, result);
        }

        public override Task<IQueryable<TResult>> QueryById<TResult>(object key, CancellationToken cancellationToken = default)
            => QueryById<PostEntity, TResult, int>(key, cancellationToken);

        protected override async Task LoadAggregate(PostEntity entity, CancellationToken cancellationToken = default)
        {
            await dbContext.Entry(entity).Collection(e => e.Tags).LoadAsync(cancellationToken);
        }
    }
}
