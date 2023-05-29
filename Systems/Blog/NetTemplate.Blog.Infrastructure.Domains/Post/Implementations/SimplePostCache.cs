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

        public async Task<PostView> GetEntryOrAdd(int id, string currentVersion, Func<Task<PostView>> createFunc, TimeSpan? expiration = null)
        {
            PostView entry = null;
            bool needUpdate = false;
            string storedVersion = await _applicationCache.Get<string>(GetCacheVersionKey(id));
            string postVersion = null;
            string categoryVersion = null;

            if (storedVersion == currentVersion)
            {
                entry = await _applicationCache.Get<PostView>(GetCacheKey(id));
            }

            if (entry != null)
            {
                if (!needUpdate)
                {
                    postVersion = await _entityVersionManager.GetVersion(nameof(PostEntity), entry.Id.ToString());

                    if (entry.Version != postVersion)
                    {
                        needUpdate = true;
                    }
                }

                if (!needUpdate)
                {
                    categoryVersion = await _entityVersionManager.GetVersion(nameof(PostCategoryEntity), entry.CategoryId.ToString());

                    if (entry.Category?.Version != categoryVersion)
                    {
                        needUpdate = true;
                    }
                }

                // [NOTE] Others
            }
            else
            {
                needUpdate = true;
            }

            if (needUpdate)
            {
                entry = await createFunc();

                if (entry != null)
                {
                    await UpdateEntry(entry, currentVersion, postVersion, categoryVersion, expiration);
                }
            }

            return entry;
        }

        public async Task<bool> RemoveEntry(int id)
        {
            await _applicationCache.Remove(GetCacheKey(id));
            return true;
        }

        public async Task<bool> UpdateEntry(PostView entry, string currentVersion, TimeSpan? expiration = null)
        {
            return await UpdateEntry(entry, currentVersion, postVersion: null, categoryVersion: null, expiration);
        }

        private async Task<string> GetVersionIfNull(string entityName, string key, string currentVersion = null)
            => currentVersion ?? await _entityVersionManager.GetVersion(entityName, key);

        private async Task<bool> UpdateEntry(PostView entry, string currentVersion,
            string postVersion, string categoryVersion,
            TimeSpan? expiration = null)
        {
            entry.Version = await GetVersionIfNull(nameof(PostEntity), entry.Id.ToString(), postVersion);

            if (entry.Category != null)
            {
                entry.Category.Version = await GetVersionIfNull(nameof(PostCategoryEntity), entry.CategoryId.ToString(), categoryVersion);
            }

            await _applicationCache.TrySet(GetCacheVersionKey(entry.Id), currentVersion, expiration);
            await _applicationCache.TrySet(GetCacheKey(entry.Id), entry, expiration);

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
