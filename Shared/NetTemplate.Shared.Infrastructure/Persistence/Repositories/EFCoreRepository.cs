using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
            DbSet<T> query = DbSet;

            IQueryable<TResult> result = mapper.CustomProjectTo<TResult>(query);

            return Task.FromResult(result);
        }

        protected abstract Task LoadAggregate(T entity, CancellationToken cancellationToken = default);

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
    }

    public abstract class EFCoreRepository<T, TId, TDbContext>
        : EFCoreRepository<T, TDbContext>, IRepository<T, TId>
        where T : class, IAggregateRoot, IHasId<TId>
        where TDbContext : DbContext
    {
        protected EFCoreRepository(TDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public virtual async Task<bool> Exists(TId key, CancellationToken cancellationToken = default)
        {
            bool exists = await DbSet.Where(e => Equals(e.Id, key)).AnyAsync(cancellationToken);

            return exists;
        }

        public virtual async Task<T> FindById(TId key, CancellationToken cancellationToken = default)
        {
            T entity = await DbSet.ById(key).FirstOrDefaultAsync(cancellationToken);

            await LoadAggregate(entity, cancellationToken);

            return entity;
        }

        public virtual Task<IQueryable<TResult>> QueryById<TResult>(TId key, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = DbSet.ById(key);

            IQueryable<TResult> result = mapper.ProjectTo<TResult>(query);

            return Task.FromResult(result);
        }

        public virtual Task<IQueryable<TResult>> QueryByIds<TResult>(IEnumerable<TId> keys, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = DbSet.ByIds(keys);

            IQueryable<TResult> result = mapper.ProjectTo<TResult>(query);

            return Task.FromResult(result);
        }
    }
}
