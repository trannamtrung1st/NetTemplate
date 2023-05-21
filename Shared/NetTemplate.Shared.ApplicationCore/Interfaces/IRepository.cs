using NetTemplate.Shared.ApplicationCore.Entities;

namespace NetTemplate.Shared.ApplicationCore.Interfaces
{
    public interface IRepository<T> where T : class, IAggregateRoot
    {
        IQueryable<T> GetQuery();
        Task<T> FindById(params object[] keys);
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(T entity);
        Task<T> Track(T entity);
    }
}
