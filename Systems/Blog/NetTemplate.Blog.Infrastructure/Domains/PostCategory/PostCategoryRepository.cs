using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetTemplate.Blog.ApplicationCore.Common.Utils;
using NetTemplate.Blog.ApplicationCore.PostCategory;
using NetTemplate.Blog.Infrastructure.Persistence;
using NetTemplate.Common.DependencyInjection.Attributes;
using NetTemplate.Common.Mapping.Extensions;
using NetTemplate.Common.Queries.Extensions;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Models;
using NetTemplate.Shared.Infrastructure.Persistence.Repositories;

namespace NetTemplate.Blog.Infrastructure.Domains.PostCategory
{
    [ScopedService]
    public class PostCategoryRepository : EFCoreRepository<PostCategoryEntity, int, MainDbContext>, IPostCategoryRepository
    {
        public PostCategoryRepository(MainDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<bool> NameExists(string name, CancellationToken cancellationToken = default)
        {
            bool exists = await DbSet.Where(e => e.Name == name).AnyAsync(cancellationToken);

            return exists;
        }

        public async Task<QueryResponseModel<TResult>> Query<TResult>(
            string terms = null,
            IEnumerable<int> ids = null,
            Enums.PostCategorySortBy[] sortBy = null,
            bool[] isDesc = null,
            IOffsetPagingQuery paging = null,
            bool count = true,
            CancellationToken cancellationToken = default)
        {
            IQueryable<PostCategoryEntity> query = DbSet;

            if (!string.IsNullOrEmpty(terms))
            {
                query = query.Where(e => e.Name.Contains(terms));
            }

            query = query.ByIdsIfAny(ids);

            int? total = null;

            if (count)
            {
                total = await query.CountAsync(cancellationToken);
            }

            query = query.SortBy(sortBy, isDesc,
                Process: (query, sort, isDesc) => sort switch
                {
                    Enums.PostCategorySortBy.CreatorFullName => query.SortSequential(EntityHelper.GetCreatorFullNameExpression<PostCategoryEntity>(), isDesc),
                    _ => null,
                });

            query = query.OffsetPaging(paging);

            IQueryable<TResult> result = mapper.CustomProjectTo<TResult>(query);

            return new QueryResponseModel<TResult>(total, result);
        }

        protected override Task LoadAggregate(PostCategoryEntity entity, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
