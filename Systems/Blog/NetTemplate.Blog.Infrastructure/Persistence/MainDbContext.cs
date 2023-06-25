using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTemplate.Blog.ApplicationCore.Comment;
using NetTemplate.Blog.ApplicationCore.Post;
using NetTemplate.Blog.ApplicationCore.PostCategory;
using NetTemplate.Blog.ApplicationCore.User;
using NetTemplate.Shared.ApplicationCore.Identity.Interfaces;
using NetTemplate.Shared.Infrastructure.Persistence;

namespace NetTemplate.Blog.Infrastructure.Persistence
{
    public class MainDbContext : BaseDbContext<MainDbContext>
    {
        public static readonly List<Func<MainDbContext, IServiceProvider, CancellationToken, Task>> MigrationSeedingActions
            = new List<Func<MainDbContext, IServiceProvider, CancellationToken, Task>>();

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

        protected override List<Func<MainDbContext, IServiceProvider, CancellationToken, Task>> GetMigrationSeedingActions()
            => MigrationSeedingActions;
    }
}
