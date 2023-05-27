using NetTemplate.Blog.ApplicationCore.PostCategory;
using NetTemplate.Blog.Infrastructure.Persistence;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.Infrastructure.Persistence.Repositories;

namespace NetTemplate.Blog.Infrastructure.Domains.PostCategory
{
    [ScopedService]
    public class PostCategoryRepository : EFCoreRepository<PostCategoryEntity, MainDbContext>, IPostCategoryRepository
    {
        public PostCategoryRepository(MainDbContext dbContext) : base(dbContext)
        {
        }

        protected override Task LoadAggregate(PostCategoryEntity entity)
        {
            return Task.CompletedTask;
        }
    }
}
