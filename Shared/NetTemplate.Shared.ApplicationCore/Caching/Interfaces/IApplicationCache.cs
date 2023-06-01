namespace NetTemplate.Shared.ApplicationCore.Caching.Interfaces
{
    public interface IApplicationCache
    {
        Task Set<T>(string cacheKey, T cacheValue, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
        Task<T> GetOrAdd<T>(string cacheKey, Func<Task<T>> createFunc, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
        Task<T> Get<T>(string cacheKey, CancellationToken cancellationToken = default);
        Task Remove(string cacheKey, CancellationToken cancellationToken = default);
        Task<bool> Exists(string cacheKey, CancellationToken cancellationToken = default);
    }
}
