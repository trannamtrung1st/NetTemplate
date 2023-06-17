using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetTemplate.Blog.ApplicationCore.Post;
using NetTemplate.Blog.Infrastructure.Persistence;
using NetTemplate.Common.DependencyInjection.Attributes;
using NetTemplate.Common.Mapping.Extensions;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Models;
using NetTemplate.Shared.Infrastructure.Persistence.Repositories;

namespace NetTemplate.Blog.Infrastructure.Domains.Post
{
    [ScopedService]
    public class PostRepository : EFCoreRepository<PostEntity, int, MainDbContext>, IPostRepository
    {
        public PostRepository(MainDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<int> CountByCategory(int id, CancellationToken cancellationToken = default)
        {
            int count = await DbSet.Where(e => e.CategoryId == id).CountAsync(cancellationToken);

            return count;
        }

        public override async Task<PostEntity> FindById(int key, CancellationToken cancellationToken = default)
        {
            PostEntity entity = await DbSet.ById(key)
                .Select(PostEntity.SelectBasicInfoExpression)
                .FirstOrDefaultAsync(cancellationToken);

            if (entity != null)
            {
                await Track(entity, cancellationToken);

                await LoadAggregate(entity, cancellationToken);
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

        public async Task<bool> TitleExists(string title, CancellationToken cancellationToken = default)
        {
            bool exists = await DbSet.Where(e => e.Title == title).AnyAsync(cancellationToken);

            return exists;
        }

        protected override async Task LoadAggregate(PostEntity entity, CancellationToken cancellationToken = default)
        {
            await dbContext.Entry(entity).Collection(e => e.Tags).LoadAsync(cancellationToken);
        }
    }
}
