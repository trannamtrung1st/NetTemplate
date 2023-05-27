using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTemplate.Blog.ApplicationCore.Comment;
using NetTemplate.Blog.ApplicationCore.Post;
using NetTemplate.Blog.ApplicationCore.PostCategory;
using NetTemplate.Blog.ApplicationCore.User;
using NetTemplate.Shared.ApplicationCore.Identity.Interfaces;
using NetTemplate.Shared.Infrastructure.Persistence;
using NetTemplate.Shared.Infrastructure.Persistence.Extensions;

namespace NetTemplate.Blog.Infrastructure.Persistence
{
    public class MainDbContext : BaseDbContext<MainDbContext>
    {
        // [IMPORTANT] It is acceptable to comment obsolete migration logics (e.g, Changes in properties)
        public static readonly List<Func<MainDbContext, IServiceProvider, Task>> MigrationSeedingActions
            = new List<Func<MainDbContext, IServiceProvider, Task>>();

        public MainDbContext(ICurrentUserProvider currentUserProvider) : base(currentUserProvider)
        {
        }

        public MainDbContext(
            DbContextOptions<MainDbContext> options,
            ICurrentUserProvider currentUserProvider) : base(options, currentUserProvider)
        {
        }

        public MainDbContext(
            DbContextOptions<MainDbContext> options,
            IMediator mediator,
            ICurrentUserProvider currentUserProvider) : base(options, mediator, currentUserProvider)
        {
        }

        public virtual DbSet<PostEntity> Post { get; set; }
        public virtual DbSet<PostTagEntity> PostTag { get; set; }
        public virtual DbSet<PostCategoryEntity> PostCategory { get; set; }
        public virtual DbSet<CommentEntity> Comment { get; set; }
        public virtual DbSet<UserPartialEntity> UserPartial { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public async Task SeedMigrationsAsync(IServiceProvider serviceProvider)
        {
            using (var transaction = await this.BeginTransactionOrCurrent())
            {
                foreach (var action in MigrationSeedingActions)
                {
                    await action(this, serviceProvider);
                }

                MigrationSeedingActions.Clear();

                await transaction.CommitAsync();
            }
        }
    }
}
