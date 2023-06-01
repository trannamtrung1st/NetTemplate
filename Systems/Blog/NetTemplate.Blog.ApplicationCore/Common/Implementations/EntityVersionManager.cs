using NetTemplate.Blog.ApplicationCore.Common.Interfaces;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.ApplicationCore.Caching.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.Common.Implementations
{
    [ScopedService]
    public class EntityVersionManager : IEntityVersionManager
    {
        private readonly IApplicationCache _applicationCache;

        public EntityVersionManager(IApplicationCache applicationCache)
        {
            _applicationCache = applicationCache;
        }

        public async Task<string> UpdateVersion(string entityName, string key, CancellationToken cancellationToken = default)
        {
            string cacheKey = GetKey(entityName, key);

            string newVersion = GetNewVersion();

            await _applicationCache.Set(cacheKey, newVersion, cancellationToken: cancellationToken);

            return newVersion;
        }

        public async Task<string> GetVersion(string entityName, string key, CancellationToken cancellationToken = default)
        {
            string cacheKey = GetKey(entityName, key);

            return await _applicationCache.GetOrAdd(cacheKey, () => Task.FromResult(GetNewVersion()), cancellationToken: cancellationToken);
        }

        public async Task Remove(string entityName, string key, CancellationToken cancellationToken = default)
        {
            string cacheKey = GetKey(entityName, key);

            await _applicationCache.Remove(cacheKey, cancellationToken: cancellationToken);
        }

        private static string GetNewVersion() => DateTime.UtcNow.Ticks.ToString();

        private static string GetKey(string entityName, string key)
            => Constants.EntityVersionCacheKeyTemplate.Replace("{EntityName}", entityName).Replace("{Key}", key);

        private static class Constants
        {
            public const string EntityVersionCacheKeyTemplate = $"{nameof(EntityVersionManager)}_{{EntityName}}_{{Key}}";
        }
    }
}
