using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetTemplate.Shared.ApplicationCore.Common.Entities;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using NetTemplate.Shared.Infrastructure.Persistence.Extensions;

namespace NetTemplate.Shared.Infrastructure.Persistence.Repositories
{
    public abstract class EFCoreRepository<T, TDbContext>
        : IRepository<T>
        where T : class, IAggregateRoot
        where TDbContext : DbContext
    {
        private static readonly EntityState[] CanUpdateStates = new[]
        {
            EntityState.Detached,
            EntityState.Unchanged,
            EntityState.Modified
        };

        protected readonly TDbContext dbContext;

        public EFCoreRepository(TDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<T> GetQuery() => dbContext.Set<T>();

        protected abstract Task LoadAggregate(T entity);

        public virtual async Task<T> FindById(params object[] keys)
        {
            T entity = await dbContext.FindAsync<T>(keys);

            await LoadAggregate(entity);

            return entity;
        }

        public virtual async Task<T> Create(T entity)
        {
            EntityEntry<T> entry = dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                return (await dbContext.AddAsync(entity)).Entity;
            }

            return entity;
        }

        public virtual Task<T> Update(T entity)
        {
            EntityEntry<T> entry = dbContext.Entry(entity);

            if (CanUpdateStates.Contains(entry.State))
            {
                return Task.FromResult(dbContext.Update(entity).Entity);
            }

            return Task.FromResult(entity);
        }

        public virtual Task<T> Delete(T entity)
        {
            entity = dbContext.Remove(entity).Entity;

            return Task.FromResult(entity);
        }

        public virtual Task<T> Track(T entity)
        {
            dbContext.TryAttach(entity, out _);

            return Task.FromResult(entity);
        }

        public static TId GetIdFromKeys<TId>(object[] keys)
        {
            if (keys?.Length == 1 && keys[0] is TId id)
            {
                return id;
            }

            throw new ArgumentException(null, nameof(keys));
        }
    }
}
