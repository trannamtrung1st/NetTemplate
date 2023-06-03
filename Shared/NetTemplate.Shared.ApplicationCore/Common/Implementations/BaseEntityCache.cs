using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Shared.ApplicationCore.Common.Implementations
{
    public abstract class BaseEntityCache<T, TData> : IEntityCache<T>
    {
        private readonly IEntityVersionManager _entityVersionManager;

        public BaseEntityCache(IEntityVersionManager entityVersionManager)
        {
            _entityVersionManager = entityVersionManager;
        }

        public virtual async Task<T> GetEntryOrAdd(string entityKey, string currentVersion, Func<CancellationToken, Task<T>> createFunc, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            T entry = default;
            bool needUpdate;
            string storedVersion = await GetStoredVersion(entityKey, cancellationToken);
            EntityCacheValidationModel<TData> validationModel = null;

            if (storedVersion == currentVersion)
            {
                entry = await GetCachedEntry(entityKey, cancellationToken);
            }

            if (entry != null)
            {
                validationModel = await ValidateCachedEntry(entry, cancellationToken);

                needUpdate = validationModel.NeedUpdate;
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
                    await UpdateEntry(entityKey, entry, currentVersion, validationModel, expiration, cancellationToken);
                }
                else
                {
                    await RemoveEntry(entityKey, cancellationToken);
                }
            }

            return entry;
        }

        public abstract Task<bool> RemoveEntry(string entityKey, CancellationToken cancellationToken = default);

        public virtual async Task<bool> UpdateEntry(string entityKey, T entry, string currentVersion, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            return await UpdateEntry(entityKey, entry, currentVersion, validationModel: null, expiration, cancellationToken);
        }

        protected virtual async Task<string> GetVersionIfNull(string entityName, string entityKey, string currentVersion = null, CancellationToken cancellationToken = default)
            => currentVersion ?? await _entityVersionManager.GetVersion(entityName, entityKey, cancellationToken);

        protected abstract Task<string> GetStoredVersion(string entityKey, CancellationToken cancellationToken = default);
        protected abstract Task<T> GetCachedEntry(string entityKey, CancellationToken cancellationToken = default);
        protected abstract Task<EntityCacheValidationModel<TData>> ValidateCachedEntry(T entry, CancellationToken cancellationToken = default);
        protected abstract Task<bool> UpdateEntry(
            string entityKey, T entry, string version, EntityCacheValidationModel<TData> validationModel,
            TimeSpan? expiration = null, CancellationToken cancellationToken = default);
    }
}
