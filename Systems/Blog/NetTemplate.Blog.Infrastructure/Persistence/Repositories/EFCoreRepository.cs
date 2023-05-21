using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetTemplate.Blog.Infrastructure.Persistence.Extensions;
using NetTemplate.Shared.ApplicationCore.Common.Entities;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.Infrastructure.Persistence.Repositories
{
    public abstract class EFCoreRepository<T> : IRepository<T> where T : class, IAggregateRoot
    {
        private static readonly EntityState[] CanUpdateStates = new[]
        {
            EntityState.Detached,
            EntityState.Unchanged,
            EntityState.Modified
        };

        protected readonly MainDbContext dbContext;

        public EFCoreRepository(MainDbContext dbContext)
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

        public async Task<T> Create(T entity)
        {
            EntityEntry<T> entry = dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                return (await dbContext.AddAsync(entity)).Entity;
            }

            return entity;
        }

        public Task<T> Update(T entity)
        {
            EntityEntry<T> entry = dbContext.Entry(entity);

            if (CanUpdateStates.Contains(entry.State))
            {
                return Task.FromResult(dbContext.Update(entity).Entity);
            }

            return Task.FromResult(entity);
        }

        public Task<T> Delete(T entity)
        {
            entity = dbContext.Remove(entity).Entity;

            return Task.FromResult(entity);
        }

        public Task<T> Track(T entity)
        {
            dbContext.TryAttach(entity, out _);

            return Task.FromResult(entity);
        }
    }
}
