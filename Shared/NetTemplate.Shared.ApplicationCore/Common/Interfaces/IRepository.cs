using NetTemplate.Shared.ApplicationCore.Common.Entities;

namespace NetTemplate.Shared.ApplicationCore.Common.Interfaces
{
    public interface IRepository<T> where T : class, IAggregateRoot
    {
        Task<IQueryable<TResult>> QueryAll<TResult>(CancellationToken cancellationToken = default);
        Task<IQueryable<TResult>> QueryById<TResult>(object key, CancellationToken cancellationToken = default);
        Task<T> FindById(params object[] keys);
        Task<T> Create(T entity, CancellationToken cancellationToken = default);
        Task<T> Update(T entity, CancellationToken cancellationToken = default);
        Task<T> Delete(T entity, CancellationToken cancellationToken = default);
        Task<T> Track(T entity, CancellationToken cancellationToken = default);
    }
}
