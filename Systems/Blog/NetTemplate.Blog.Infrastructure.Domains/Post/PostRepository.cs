using NetTemplate.Blog.ApplicationCore.Post;
using NetTemplate.Blog.Infrastructure.Persistence;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.Infrastructure.Persistence.Repositories;

namespace NetTemplate.Blog.Infrastructure.Domains.Post
{
    [ScopedService]
    public class PostRepository : EFCoreRepository<PostEntity, MainDbContext>, IPostRepository
    {
        public PostRepository(MainDbContext dbContext) : base(dbContext)
        {
        }

        protected override async Task LoadAggregate(PostEntity entity)
        {
            await dbContext.Entry(entity).Collection(e => e.Tags).LoadAsync();
        }
    }
}
