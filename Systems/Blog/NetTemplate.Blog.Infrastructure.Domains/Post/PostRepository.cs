using NetTemplate.Blog.ApplicationCore.Post;
using NetTemplate.Blog.Infrastructure.Persistence;
using NetTemplate.Blog.Infrastructure.Persistence.Repositories;
using NetTemplate.Common.DependencyInjection;

namespace NetTemplate.Blog.Infrastructure.Domains.Post
{
    [ScopedService]
    public class PostRepository : EFCoreRepository<PostEntity>, IPostRepository
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
