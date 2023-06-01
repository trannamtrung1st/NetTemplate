using NetTemplate.Blog.ApplicationCore.Cross.Interfaces;
using NetTemplate.Blog.ApplicationCore.Post;
using NetTemplate.Blog.ApplicationCore.Post.Interfaces;
using NetTemplate.Blog.ApplicationCore.Post.Views;
using NetTemplate.Blog.ApplicationCore.PostCategory;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.ApplicationCore.Caching.Interfaces;
using ViewPreservedKeys = NetTemplate.Shared.ApplicationCore.Common.Constants.ViewPreservedKeys;

namespace NetTemplate.Blog.Infrastructure.Domains.Post.Implementations
{
    [ScopedService]
    public class SimplePostCache : IPostCache
    {
        private readonly IApplicationCache _applicationCache;
        private readonly IEntityVersionManager _entityVersionManager;

        public SimplePostCache(IApplicationCache applicationCache,
            IEntityVersionManager entityVersionManager)
        {
            _applicationCache = applicationCache;
            _entityVersionManager = entityVersionManager;
        }

        public async Task<PostView> GetEntryOrAdd(int id, string currentVersion, Func<CancellationToken, Task<PostView>> createFunc, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            PostView entry = null;
            bool needUpdate = false;
            string storedVersion = await _applicationCache.Get<string>(GetCacheVersionKey(id), cancellationToken);
            string postVersion = null;
            string categoryVersion = null;

            if (storedVersion == currentVersion)
            {
                entry = await _applicationCache.Get<PostView>(GetCacheKey(id), cancellationToken);
            }

            if (entry != null)
            {
                if (!needUpdate)
                {
                    postVersion = await _entityVersionManager.GetVersion(nameof(PostEntity), entry.Id.ToString(), cancellationToken);

                    if (entry.Version != postVersion)
                    {
                        needUpdate = true;
                    }
                }

                if (!needUpdate)
                {
                    categoryVersion = await _entityVersionManager.GetVersion(nameof(PostCategoryEntity), entry.CategoryId.ToString(), cancellationToken);

                    if (entry.Category?.Version != categoryVersion)
                    {
                        needUpdate = true;
                    }
                }
            }
            else
            {
                needUpdate = true;
            }

            if (needUpdate)
            {
                entry = await createFunc(cancellationToken);

                if (entry != null)
                {
                    await UpdateEntry(entry, currentVersion, postVersion, categoryVersion, expiration, cancellationToken);
                }
                else
                {
                    await RemoveEntry(id, cancellationToken);
                }
            }

            return entry;
        }

        public async Task<bool> RemoveEntry(int id, CancellationToken cancellationToken = default)
        {
            await _applicationCache.Remove(GetCacheKey(id), cancellationToken);
            return true;
        }

        public async Task<bool> UpdateEntry(PostView entry, string currentVersion, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            return await UpdateEntry(entry, currentVersion, postVersion: null, categoryVersion: null, expiration, cancellationToken: cancellationToken);
        }

        private async Task<string> GetVersionIfNull(string entityName, string key, string currentVersion = null, CancellationToken cancellationToken = default)
            => currentVersion ?? await _entityVersionManager.GetVersion(entityName, key, cancellationToken);

        private async Task<bool> UpdateEntry(PostView entry, string currentVersion,
            string postVersion, string categoryVersion,
            TimeSpan? expiration = null,
            CancellationToken cancellationToken = default)
        {
            entry.SetVersion(await GetVersionIfNull(nameof(PostEntity), entry.Id.ToString(), postVersion, cancellationToken));

            if (entry.Category != null)
            {
                entry.Category.SetVersion(await GetVersionIfNull(nameof(PostCategoryEntity), entry.CategoryId.ToString(), categoryVersion, cancellationToken));
            }

            await _applicationCache.Set(GetCacheVersionKey(entry.Id), currentVersion, expiration, cancellationToken);
            await _applicationCache.Set(GetCacheKey(entry.Id), entry, expiration, cancellationToken);

            return true;
        }

        private static string GetCacheKey(int id) => $"{Constants.PostViewPrefix}{id}";
        private static string GetCacheVersionKey(int id) => $"{Constants.PostViewVersionPrefix}{id}";

        private static class Constants
        {
            public const string PostViewPrefix = $"{nameof(SimplePostCache)}_{nameof(PostView)}_";
            public const string PostViewVersionPrefix = $"{ViewPreservedKeys.Version}_{PostViewPrefix}";
        }
    }
}
