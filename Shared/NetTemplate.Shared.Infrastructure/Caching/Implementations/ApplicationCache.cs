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

        public Task Set<T>(string cacheKey, T cacheValue, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            return _easyCachingProvider.SetAsync(cacheKey, cacheValue, expiration ?? DefaultExpiration, cancellationToken);
        }

        public async Task<T> GetOrAdd<T>(string cacheKey, Func<Task<T>> createFunc, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            var cacheValue = await _easyCachingProvider.GetAsync(cacheKey, createFunc, expiration ?? DefaultExpiration, cancellationToken);
            return cacheValue.HasValue ? cacheValue.Value : default;
        }

        public async Task<T> Get<T>(string cacheKey, CancellationToken cancellationToken = default)
        {
            var cacheValue = await _easyCachingProvider.GetAsync<T>(cacheKey, cancellationToken);
            return cacheValue.HasValue ? cacheValue.Value : default;
        }

        public Task Remove(string cacheKey, CancellationToken cancellationToken = default)
        {
            return _easyCachingProvider.RemoveAsync(cacheKey, cancellationToken);
        }

        public Task ClearAllAsync(CancellationToken cancellationToken = default)
        {
            return _easyCachingProvider.FlushAsync(cancellationToken);
        }

        public Task<bool> Exists(string cacheKey, CancellationToken cancellationToken = default)
        {
            return _easyCachingProvider.ExistsAsync(cacheKey, cancellationToken);
        }
    }
}
