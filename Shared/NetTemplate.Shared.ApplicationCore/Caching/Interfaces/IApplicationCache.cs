namespace NetTemplate.Shared.ApplicationCore.Caching.Interfaces
{
    public interface IApplicationCache
    {
        Task Set<T>(string cacheKey, T cacheValue, TimeSpan? expiration = null);
        Task<T> GetOrAdd<T>(string cacheKey, Func<Task<T>> createFunc, TimeSpan? expiration = null);
        Task<T> Get<T>(string cacheKey);
        Task Remove(string cacheKey);
        Task<bool> Exists(string cacheKey);
    }
}
