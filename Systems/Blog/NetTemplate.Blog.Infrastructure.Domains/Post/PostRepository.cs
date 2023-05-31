using Microsoft.EntityFrameworkCore;
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

        // [TODO] add cancellation tokens
        public override async Task<PostEntity> FindById(params object[] keys)
        {
            if (keys?.Length != 1 || keys[0] is not int id)
            {
                throw new ArgumentException(null, nameof(keys));
            }

            PostEntity entity = await dbContext.Post.ById(id)
                .Select(PostEntity.SelectBasicInfoExpression)
                .FirstOrDefaultAsync();

            if (entity != null)
            {
                await Track(entity);

                await LoadAggregate(entity);
            }

            return entity;
        }

        protected override async Task LoadAggregate(PostEntity entity)
        {
            await dbContext.Entry(entity).Collection(e => e.Tags).LoadAsync();
        }
    }
}
