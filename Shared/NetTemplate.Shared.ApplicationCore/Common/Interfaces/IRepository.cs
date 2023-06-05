namespace NetTemplate.Shared.ApplicationCore.Common.Interfaces
{
    public interface IRepository<T> where T : class, IAggregateRoot
    {
        Task<IQueryable<TResult>> QueryAll<TResult>(CancellationToken cancellationToken = default);
        Task<T> Create(T entity, CancellationToken cancellationToken = default);
        Task<T> Update(T entity, CancellationToken cancellationToken = default);
        Task<T> Delete(T entity, CancellationToken cancellationToken = default);
        Task<T> Track(T entity, CancellationToken cancellationToken = default);
    }

    public interface IRepository<T, TId> : IRepository<T> where T : class, IAggregateRoot, IHasId<TId>
    {
        Task<IQueryable<TResult>> QueryById<TResult>(TId key, CancellationToken cancellationToken = default);
        Task<IQueryable<TResult>> QueryByIds<TResult>(IEnumerable<TId> keys, CancellationToken cancellationToken = default);
        Task<bool> Exists(TId key, CancellationToken cancellationToken = default);
        Task<T> FindById(TId key, CancellationToken cancellationToken = default);
    }
}
