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
                string versionKey = $"{cacheKey}{ViewPreservedKeys.Version}";

                string storedVersion = await _memoryStore.StringGet<string>(versionKey);

                if (storedVersion != currentVersion)
                {
                    await action();

                    await _memoryStore.StringSet(versionKey, currentVersion);
                }
            }
        }
    }
}
