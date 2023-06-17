using NetTemplate.Common.MemoryStore.Interfaces;
using ViewPreservedKeys = NetTemplate.Shared.ApplicationCore.Common.Constants.ViewPreservedKeys;

namespace NetTemplate.Shared.ApplicationCore.Common.Implementations
{
    public abstract class BaseViewManager
    {
        private readonly IDistributedMemoryStore _memoryStore;

        public BaseViewManager(IDistributedMemoryStore memoryStore)
        {
            _memoryStore = memoryStore;
        }

        protected virtual async Task Initialize(string cacheKey, string currentVersion,
            Func<CancellationToken, Task> Rebuild,
            Func<CancellationToken, Task> OnFinish,
            CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(currentVersion))
            {
                string storedVersion = await _memoryStore.HashGet(cacheKey, ViewPreservedKeys.Version, cancellationToken);

                if (storedVersion != currentVersion)
                {
                    await Rebuild(cancellationToken);

                    await _memoryStore.HashSet(cacheKey, ViewPreservedKeys.Version, currentVersion, cancellationToken);
                }

                if (OnFinish != null)
                {
                    await OnFinish(cancellationToken);
                }
            }
        }
    }
}
