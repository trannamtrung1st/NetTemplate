using EasyCaching.Core;
using NetTemplate.Shared.ApplicationCore.Caching.Interfaces;

namespace NetTemplate.Shared.Infrastructure.Caching.Implementations
{
    public class ApplicationCache : IApplicationCache
    {
        static readonly TimeSpan DefaultExpiration = TimeSpan.FromHours(1);

        private readonly IEasyCachingProvider _easyCachingProvider;

        public ApplicationCache(IEasyCachingProvider easyCachingProvider)
        {
            _easyCachingProvider = easyCachingProvider;
        }

        public Task Set<T>(string cacheKey, T cacheValue, TimeSpan? expiration = null)
        {
            return _easyCachingProvider.SetAsync(cacheKey, cacheValue, expiration ?? DefaultExpiration);
        }

        public async Task<T> GetOrAdd<T>(string cacheKey, Func<Task<T>> createFunc, TimeSpan? expiration = null)
        {
            var cacheValue = await _easyCachingProvider.GetAsync(cacheKey, createFunc, expiration ?? DefaultExpiration);
            return cacheValue.HasValue ? cacheValue.Value : default;
        }

        public async Task<T> Get<T>(string cacheKey)
        {
            var cacheValue = await _easyCachingProvider.GetAsync<T>(cacheKey);
            return cacheValue.HasValue ? cacheValue.Value : default;
        }

        public Task Remove(string cacheKey)
        {
            return _easyCachingProvider.RemoveAsync(cacheKey);
        }

        public Task ClearAllAsync()
        {
            return _easyCachingProvider.FlushAsync();
        }

        public Task<bool> Exists(string cacheKey)
        {
            return _easyCachingProvider.ExistsAsync(cacheKey);
        }
    }
}
