using AutoMapper;
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
        protected readonly IMapper mapper;

        public EFCoreRepository(TDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        protected DbSet<T> DbSet => dbContext.Set<T>();

        public Task<IQueryable<TResult>> QueryAll<TResult>(CancellationToken cancellationToken = default)
        {
            DbSet<T> query = dbContext.Set<T>();

            IQueryable<TResult> result = mapper.CustomProjectTo<TResult>(query);

            return Task.FromResult(result);
        }

        protected abstract Task LoadAggregate(T entity, CancellationToken cancellationToken = default);

        public virtual async Task<T> FindById(params object[] keys)
        {
            T entity = await dbContext.FindAsync<T>(keys);

            await LoadAggregate(entity);

            return entity;
        }

        public virtual async Task<T> Create(T entity, CancellationToken cancellationToken = default)
        {
            EntityEntry<T> entry = dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                return (await dbContext.AddAsync(entity, cancellationToken)).Entity;
            }

            return entity;
        }

        public virtual Task<T> Update(T entity, CancellationToken cancellationToken = default)
        {
            EntityEntry<T> entry = dbContext.Entry(entity);

            if (CanUpdateStates.Contains(entry.State))
            {
                return Task.FromResult(dbContext.Update(entity).Entity);
            }

            return Task.FromResult(entity);
        }

        public virtual Task<T> Delete(T entity, CancellationToken cancellationToken = default)
        {
            entity = dbContext.Remove(entity).Entity;

            return Task.FromResult(entity);
        }

        public virtual Task<T> Track(T entity, CancellationToken cancellationToken = default)
        {
            dbContext.TryAttach(entity, out _);

            return Task.FromResult(entity);
        }

        public abstract Task<IQueryable<TResult>> QueryById<TResult>(object key, CancellationToken cancellationToken = default);

        protected Task<IQueryable<TResult>> QueryById<TEntity, TResult, TId>(object key, CancellationToken cancellationToken = default)
            where TEntity : class, IHasId<TId>
        {
            TId id = GetIdFromObject<TId>(key);

            IQueryable<TEntity> query = dbContext.Set<TEntity>().ById(id);

            IQueryable<TResult> result = mapper.CustomProjectTo<TResult>(query);

            return Task.FromResult(result);
        }

        public static TId GetIdFromKeys<TId>(object[] keys)
        {
            if (keys?.Length == 1 && keys[0] is TId id)
            {
                return id;
            }

            throw new ArgumentException(null, nameof(keys));
        }

        public static TId GetIdFromObject<TId>(object key)
        {
            if (key is TId id)
            {
                return id;
            }

            throw new ArgumentException(null, nameof(key));
        }
    }
}
