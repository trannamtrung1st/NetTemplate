using NetTemplate.Blog.ApplicationCore.Post;
using NetTemplate.Blog.ApplicationCore.Post.Views;
using NetTemplate.Blog.ApplicationCore.PostCategory;
using NetTemplate.Shared.ApplicationCore.Caching.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Implementations;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Models;
using ViewPreservedKeys = NetTemplate.Shared.ApplicationCore.Common.Constants.ViewPreservedKeys;

namespace NetTemplate.Blog.Infrastructure.Domains.Post.Implementations
{
    public class SimplePostCache : BaseEntityCache<PostView, (string PostVersion, string CategoryVersion)>
    {
        private readonly IApplicationCache _applicationCache;

        public SimplePostCache(IApplicationCache applicationCache,
            IEntityVersionManager entityVersionManager)
            : base(entityVersionManager)
        {
            _applicationCache = applicationCache;
        }

        public override async Task<bool> RemoveEntry(string entityKey, CancellationToken cancellationToken = default)
        {
            await _applicationCache.Remove(GetCacheKey(entityKey), cancellationToken);

            return true;
        }

        protected override async Task<string> GetStoredVersion(string entityKey, CancellationToken cancellationToken = default)
        {
            string storedVersion = await _applicationCache.Get<string>(GetCacheVersionKey(entityKey), cancellationToken);

            return storedVersion;
        }

        protected override async Task<PostView> GetCachedEntry(string entityKey, CancellationToken cancellationToken = default)
        {
            PostView entry = await _applicationCache.Get<PostView>(GetCacheKey(entityKey), cancellationToken);

            return entry;
        }

        protected override async Task<EntityCacheValidationModel<(string PostVersion, string CategoryVersion)>> ValidateCachedEntry(PostView entry, CancellationToken cancellationToken = default)
        {
            string postVersion = null;
            string categoryVersion = null;
            bool needUpdate = false;

            if (!needUpdate)
            {
                postVersion = await GetVersionIfNull(nameof(PostEntity), entry.Id.ToString(), cancellationToken: cancellationToken);

                if (entry._version_ != postVersion)
                {
                    needUpdate = true;
                }
            }

            if (!needUpdate)
            {
                categoryVersion = await GetVersionIfNull(nameof(PostCategoryEntity), entry.CategoryId.ToString(), cancellationToken: cancellationToken);

                if (entry.Category?._version_ != categoryVersion)
                {
                    needUpdate = true;
                }
            }

            return new EntityCacheValidationModel<(string PostVersion, string CategoryVersion)>
            {
                ExtraData = (postVersion, categoryVersion),
                NeedUpdate = needUpdate
            };
        }

        protected override async Task<bool> UpdateEntry(string entityKey, PostView entry, string version, EntityCacheValidationModel<(string PostVersion, string CategoryVersion)> validationModel, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            (string postVersion, string categoryVersion) = validationModel?.ExtraData ?? default;

            entry._version_ = await GetVersionIfNull(nameof(PostEntity), entry.Id.ToString(), postVersion, cancellationToken);

            if (entry.Category != null)
            {
                entry.Category._version_ = await GetVersionIfNull(nameof(PostCategoryEntity), entry.CategoryId.ToString(), categoryVersion, cancellationToken);
            }

            await _applicationCache.Set(GetCacheVersionKey(entry.Id), version, expiration, cancellationToken);
            await _applicationCache.Set(GetCacheKey(entry.Id), entry, expiration, cancellationToken);

            return true;
        }

        private static string GetCacheKey(object entityKey) => $"{Constants.PostViewPrefix}{entityKey}";
        private static string GetCacheVersionKey(object entityKey) => $"{Constants.PostViewVersionPrefix}{entityKey}";

        private static class Constants
        {
            public const string PostViewPrefix = $"{nameof(SimplePostCache)}_{nameof(PostView)}_";
            public const string PostViewVersionPrefix = $"{ViewPreservedKeys.Version}_{PostViewPrefix}";
        }
    }
}
