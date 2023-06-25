using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTemplate.Shared.ApplicationCore.Identity.Interfaces;
using NetTemplate.Shared.Infrastructure.Persistence;

namespace NetTemplate.Identity.Infrastructure.Persistence
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
