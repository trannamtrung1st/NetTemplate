using Microsoft.EntityFrameworkCore;
using NetTemplate.Blog.ApplicationCore.PostCategory;
using NetTemplate.Blog.Infrastructure.Persistence;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.ApplicationCore.Common.Models;
using NetTemplate.Shared.Infrastructure.Persistence.Repositories;

namespace NetTemplate.Blog.Infrastructure.Domains.PostCategory
{
    [ScopedService]
    public class PostCategoryRepository : EFCoreRepository<PostCategoryEntity, MainDbContext>, IPostCategoryRepository
    {
        public PostCategoryRepository(MainDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<QueryResponseModel<PostCategoryEntity>> Query(
            string terms = null,
            IEnumerable<int> ids = null,
            Enums.PostCategorySortBy[] sortBy = null,
            bool[] isDesc = null,
            IPagingQuery paging = null,
            bool count = true)
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
                total = await query.CountAsync();
            }

            query = query.SortBy(sortBy, isDesc,
                Process: (query, sort, isDesc) => sort switch
                {
                    Enums.PostCategorySortBy.CreatorFullName => query.SortSequential(PostCategoryEntity.CreatorFullNameExpression, isDesc),
                    _ => null,
                });

            query = query.Paging(paging);

            return new QueryResponseModel<PostCategoryEntity>(total, query);
        }

        public override Task<IQueryable<PostCategoryEntity>> QueryById(params object[] keys)
            => QueryById<PostCategoryEntity, int>(keys);

        protected override Task LoadAggregate(PostCategoryEntity entity)
        {
            return Task.CompletedTask;
        }
    }
}
