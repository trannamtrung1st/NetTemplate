using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using NetTemplate.Blog.Infrastructure.Persistence.Extensions;
using NetTemplate.Common.Mediator;
using NetTemplate.Shared.ApplicationCore.Entities;
using NetTemplate.Shared.ApplicationCore.Interfaces;

namespace NetTemplate.Blog.Infrastructure.Persistence
{
    public class MainDbContext : DbContext
    {
        // [IMPORTANT] It is acceptable to comment obsolete migration logics (e.g, Changes in properties)
        public static readonly List<Func<MainDbContext, IServiceProvider, Task>> MigrationSeedingActions
            = new List<Func<MainDbContext, IServiceProvider, Task>>();

        private readonly IMediator _mediator;
        private readonly ICurrentUserProvider _currentUserProvider;

        public MainDbContext(ICurrentUserProvider currentUserProvider)
        {
            _currentUserProvider = currentUserProvider;
        }

        public MainDbContext(DbContextOptions<MainDbContext> options,
            ICurrentUserProvider currentUserProvider) : base(options)
        {
            _currentUserProvider = currentUserProvider;
        }

        public MainDbContext(DbContextOptions<MainDbContext> options,
            IMediator mediator,
            ICurrentUserProvider currentUserProvider) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _currentUserProvider = currentUserProvider;
        }

        public IDbContextTransaction CurrentTransaction => Database.CurrentTransaction;
        public bool HasActiveTransaction => Database.CurrentTransaction != null;

        // [TODO]

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.ConfigureWarnings(option =>
                option.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS)
                    .Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var dbContextAssembly = typeof(MainDbContext).Assembly;

            modelBuilder.ApplyConfigurationsFromAssembly(dbContextAssembly);

            modelBuilder.RestrictDeleteBehaviour(fkPredicate:
                fk => !fk.GetConstraintName().Contains(PersistenceConstants.ConstraintNames.NoRestrictForeignKeyConstraintPostfix));

            modelBuilder.AddGlobalQueryFilter(new[] { dbContextAssembly });
        }

        #region SaveChanges
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AuditEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            AuditEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        #endregion

        public async Task SeedMigrationsAsync(IServiceProvider serviceProvider)
        {
            using (var transaction = await BeginTransactionAsync())
            {
                foreach (var action in MigrationSeedingActions)
                {
                    await action(this, serviceProvider);
                }

                MigrationSeedingActions.Clear();

                await transaction.CommitAsync();
            }
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(
            CancellationToken cancellationToken = default)
        {
            if (HasActiveTransaction) return null;

            return await Database.BeginTransactionAsync(cancellationToken);
        }

        private void AuditEntities()
        {
            var hasChanges = ChangeTracker.HasChanges();
            if (!hasChanges) return;

            var entries = ChangeTracker.Entries()
                .Where(o => o.State == EntityState.Modified ||
                    o.State == EntityState.Added).ToArray();

            foreach (var entry in entries)
            {
                var entity = entry.Entity;

                switch (entry.State)
                {
                    case EntityState.Modified:
                        {
                            var isSoftDeleted = false;

                            if (entity is ISoftDeleteEntity softDeleteEntity)
                            {
                                isSoftDeleted = entry.Property(nameof(ISoftDeleteEntity.IsDeleted)).IsModified
                                    && softDeleteEntity.IsDeleted;

                                if (isSoftDeleted)
                                {
                                    if (entity is ISoftDeleteEntity<int> userSoftDeleteEntity)
                                    {
                                        userSoftDeleteEntity.SetDeletorId(_currentUserProvider?.UserId);
                                    }
                                }
                            }

                            if (!isSoftDeleted && entity is IAuditableEntity auditableEntity)
                            {
                                auditableEntity.UpdateLastModifiedTime();

                                if (!isSoftDeleted && entity is IAuditableEntity<int> userAuditableEntity)
                                {
                                    userAuditableEntity.SetLastModifyUserId(_currentUserProvider?.UserId);
                                }
                            }
                            break;
                        }
                    case EntityState.Added:
                        {
                            if (entity is IAuditableEntity auditableEntity)
                            {
                                auditableEntity.UpdateCreatedTime();

                                if (entity is IAuditableEntity<int> userAuditableEntity)
                                {
                                    userAuditableEntity.SetCreatorId(_currentUserProvider?.UserId);
                                }
                            }
                            break;
                        }
                }
            }
        }

        public Task ResetState()
        {
            var entries = ChangeTracker.Entries().ToArray();
            foreach (var entry in entries)
            {
                entry.State = EntityState.Detached;
            }
            return Task.CompletedTask;
        }

        public async Task<int> SaveEntitiesAsync(bool dispatchEvents = true, CancellationToken cancellationToken = default)
        {
            if (dispatchEvents) await _mediator.DispatchEntityEventsAsync(this, isPost: false);

            int result = await SaveChangesAsync(cancellationToken);

            if (dispatchEvents) await _mediator.DispatchEntityEventsAsync(this, isPost: true);

            return result;
        }
    }

    public class MainDbContextFactory : IDesignTimeDbContextFactory<MainDbContext>
    {
        public MainDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MainDbContext>()
                .UseSqlServer("Server=.;Database=NetTemplateBlog;Trusted_Connection=False;User Id=sa;Password=z@123456!;MultipleActiveResultSets=true");

            return new MainDbContext(
                optionsBuilder.Options,
                new NullMediator(),
                new NullCurrentUserProvider());
        }
    }
}
