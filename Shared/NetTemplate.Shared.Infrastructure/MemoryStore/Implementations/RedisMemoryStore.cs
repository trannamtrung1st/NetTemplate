using NetTemplate.Common.MemoryStore.Interfaces;

namespace NetTemplate.Shared.Infrastructure.MemoryStore.Implementations
{
    // [TODO] redis
    public class RedisMemoryStore : IMemoryStore
    {
        public Task<T> HashGet<T>(string key, string itemKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T[]> HashGetAll<T>(string key, string[] exceptKeys = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HashRemove(string key, string itemKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HashSet<T>(string key, string itemKey, T item, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task HashSet<T>(string key, string[] itemKeys, T[] items, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> KeyExists(string key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveKey(string key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> StringGet<T>(string key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> StringSet(string key, string value, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
