using NetTemplate.Blog.ApplicationCore.Post;
using NetTemplate.Blog.ApplicationCore.Post.Views;
using NetTemplate.Blog.ApplicationCore.PostCategory;
using NetTemplate.Shared.ApplicationCore.Common.Implementations;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using ViewPreservedKeys = NetTemplate.Shared.ApplicationCore.Common.Constants.ViewPreservedKeys;

namespace NetTemplate.Blog.Infrastructure.Domains.Post.Implementations
{
    public class RedisPostCache : BaseEntityCache<PostView, (string PostVersion, string CategoryVersion)>
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public RedisPostCache(ConnectionMultiplexer connectionMultiplexer,
            IEntityVersionManager entityVersionManager)
            : base(entityVersionManager)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public override async Task<bool> RemoveEntry(string entityKey, CancellationToken cancellationToken = default)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();

            return await db.KeyDeleteAsync(GetCacheKey(entityKey));
        }

        protected override async Task<string> GetStoredVersion(string entityKey, CancellationToken cancellationToken = default)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();

            string storedVersion = await db.HashGetAsync(GetCacheKey(entityKey), ViewPreservedKeys.Version);

            return storedVersion;
        }

        protected override async Task<PostView> GetCachedEntry(string entityKey, CancellationToken cancellationToken = default)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();

            string value = await db.HashGetAsync(GetCacheKey(entityKey), ViewPreservedKeys.Entry);

            PostView entry = value != null ? JsonConvert.DeserializeObject<PostView>(value) : null;

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
            IDatabase db = _connectionMultiplexer.GetDatabase();

            (string postVersion, string categoryVersion) = validationModel?.ExtraData ?? default;

            entry._version_ = await GetVersionIfNull(nameof(PostEntity), entry.Id.ToString(), postVersion, cancellationToken);

            if (entry.Category != null)
            {
                entry.Category._version_ = await GetVersionIfNull(nameof(PostCategoryEntity), entry.CategoryId.ToString(), categoryVersion, cancellationToken);
            }

            string entryKey = GetCacheKey(entry.Id);
            ITransaction trans = db.CreateTransaction();
            _ = trans.HashSetAsync(entryKey, new[]
            {
                new HashEntry(ViewPreservedKeys.Version, version),
                new HashEntry(ViewPreservedKeys.Entry, JsonConvert.SerializeObject(entry))
            });
            _ = trans.KeyExpireAsync(entryKey, expiration);

            return await trans.ExecuteAsync();
        }

        private static string GetCacheKey(object entityKey) => $"{PostViewPrefix}{entityKey}";


        public const string PostViewPrefix = $"{nameof(RedisPostCache)}_{nameof(PostView)}_";
    }
}
