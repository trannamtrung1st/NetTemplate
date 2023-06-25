using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using NetTemplate.Shared.ApplicationCore.Identity.Interfaces;
using NetTemplate.Shared.Infrastructure.Persistence.Extensions;
using NetTemplate.Shared.Infrastructure.Persistence.QueryFilters;

namespace NetTemplate.Shared.Infrastructure.Persistence
{
    public abstract class BaseDbContext<T> : DbContext where T : DbContext
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserProvider _currentUserProvider;

        public BaseDbContext(ICurrentUserProvider currentUserProvider)
        {
            _currentUserProvider = currentUserProvider;
        }

        public BaseDbContext(DbContextOptions<T> options,
            ICurrentUserProvider currentUserProvider) : base(options)
        {
            _currentUserProvider = currentUserProvider;
        }

        public BaseDbContext(DbContextOptions<T> options,
            IMediator mediator,
            ICurrentUserProvider currentUserProvider) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _currentUserProvider = currentUserProvider;
        }

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

            var dbContextAssembly = typeof(T).Assembly;

            modelBuilder.ApplyConfigurationsFromAssembly(dbContextAssembly);

            modelBuilder.RestrictDeleteBehaviour(fkPredicate:
                fk => !fk.GetConstraintName().EndsWith(Constants.ConstraintNames.NoRestrictForeignKeyConstraintPostfix));

            modelBuilder.AddGlobalQueryFilter(new NotDeletedQueryFilter());

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

        public virtual Task ResetState(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries().ToArray();
            foreach (var entry in entries)
            {
                entry.State = EntityState.Detached;
            }
            return Task.CompletedTask;
        }

        public virtual async Task<int> SaveEntitiesAsync(bool dispatchEvents = true, CancellationToken cancellationToken = default)
        {
            if (dispatchEvents) await _mediator.DispatchEntityEventsAsync(this, isPost: false, cancellationToken);

            int result = await SaveChangesAsync(cancellationToken);

            if (dispatchEvents) await _mediator.DispatchEntityEventsAsync(this, isPost: true, cancellationToken);

            return result;
        }

        public virtual async Task SeedMigrationsAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
        {
            using var transaction = await this.BeginTransactionOrCurrent(cancellationToken);

            List<Func<T, IServiceProvider, CancellationToken, Task>> actions = GetMigrationSeedingActions();

            foreach (var action in actions)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await action(this as T, serviceProvider, cancellationToken);
            }

            actions.Clear();

            await transaction.CommitAsync(cancellationToken: cancellationToken);
        }

        public virtual async Task Migrate(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
        {
            await Database.MigrateAsync(cancellationToken);
            await SeedMigrationsAsync(serviceProvider, cancellationToken);
        }

        protected virtual void AuditEntities()
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

        protected abstract List<Func<T, IServiceProvider, CancellationToken, Task>> GetMigrationSeedingActions();
    }
}
