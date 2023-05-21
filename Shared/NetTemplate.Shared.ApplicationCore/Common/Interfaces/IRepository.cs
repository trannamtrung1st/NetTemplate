using NetTemplate.Shared.ApplicationCore.Common.Entities;

namespace NetTemplate.Shared.ApplicationCore.Common.Interfaces
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
