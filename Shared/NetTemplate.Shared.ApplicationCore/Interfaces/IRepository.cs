using NetTemplate.Shared.ApplicationCore.Entities;

namespace NetTemplate.Shared.ApplicationCore.Interfaces
{
    public interface IRepository<T> where T : class, IAggregateRoot
    {
        IQueryable<T> GetQuery();
        Task<T> FindByIdAsync(params object[] keys);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(T entity);
        Task<T> TrackAsync(T entity);
    }

    public interface ISoftDeleteRepository<T> : IRepository<T>
        where T : class, IAggregateRoot, ISoftDeleteEntity
    {
        Task<T> SoftDeleteAsync(T entity);
    }
}
