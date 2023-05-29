using NetTemplate.Common.MemoryStore.Interfaces;
using ViewPreservedKeys = NetTemplate.Shared.ApplicationCore.Common.Constants.ViewPreservedKeys;

namespace NetTemplate.Shared.ApplicationCore.Common.Implementations
{
    public abstract class BaseViewManager
    {
        private readonly IMemoryStore _memoryStore;

        public BaseViewManager(IMemoryStore memoryStore)
        {
            _memoryStore = memoryStore;
        }

        protected virtual async Task Initialize(string cacheKey, string currentVersion, Func<Task> action)
        {
            if (!string.IsNullOrWhiteSpace(currentVersion))
            {
                string storedVersion = await _memoryStore.HashGet<string>(cacheKey, ViewPreservedKeys.Version);

                if (storedVersion != currentVersion)
                {
                    await action();

                    await _memoryStore.HashSet(cacheKey, ViewPreservedKeys.Version, currentVersion);
                }
            }
        }
    }
}
