using NetTemplate.Common.MemoryStore.Interfaces;
using System.Collections.Concurrent;

namespace NetTemplate.Common.MemoryStore.Implementations
{
    public class SimpleMemoryStore : IMemoryStore
    {
        private readonly ConcurrentDictionary<string, object> _map;

        public SimpleMemoryStore()
        {
            _map = new ConcurrentDictionary<string, object>();
        }

        public Task<T> HashGet<T>(string key, string itemKey, CancellationToken cancellationToken = default)
        {
            T obj = default;

            if (_map.TryGetValue(key, out object item))
            {
                ConcurrentDictionary<string, object> hash = (ConcurrentDictionary<string, object>)item;

                if (hash.TryGetValue(itemKey, out object value))
                {
                    obj = (T)value;
                }
            }

            return Task.FromResult(obj);
        }

        public async Task<string> HashGet(string key, string itemKey, CancellationToken cancellationToken = default)
        {
            return await HashGet<string>(key, itemKey, cancellationToken);
        }

        public Task<T[]> HashGetAll<T>(string key, string[] exceptKeys = null, CancellationToken cancellationToken = default)
        {
            T[] list = default;

            if (_map.TryGetValue(key, out object item))
            {
                ConcurrentDictionary<string, object> hash = (ConcurrentDictionary<string, object>)item;

                List<T> results = new List<T>();

                foreach (KeyValuePair<string, object> kvp in hash)
                {
                    if (exceptKeys == null || exceptKeys.Length == 0 || !exceptKeys.Contains(kvp.Key))
                    {
                        results.Add((T)kvp.Value);
                    }
                }

                list = results.ToArray();
            }

            return Task.FromResult(list);
        }

        public async Task<string[]> HashGetAll(string key, string[] exceptKeys = null, CancellationToken cancellationToken = default)
        {
            return await HashGetAll<string>(key, exceptKeys, cancellationToken);
        }

        public Task<bool> HashRemove(string key, string itemKey, CancellationToken cancellationToken = default)
        {
            if (_map.TryGetValue(key, out object item))
            {
                ConcurrentDictionary<string, object> hash = (ConcurrentDictionary<string, object>)item;

                if (hash.TryRemove(itemKey, out _))
                {
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }

        public Task<bool> HashSet<T>(string key, string itemKey, T item, CancellationToken cancellationToken = default)
        {
            object itemMap = _map.GetOrAdd(key, (_) => new ConcurrentDictionary<string, object>());

            ConcurrentDictionary<string, object> hash = (ConcurrentDictionary<string, object>)itemMap;

            hash[itemKey] = item;

            return Task.FromResult(true);
        }

        public Task HashSet<T>(string key, string[] itemKeys, T[] items, CancellationToken cancellationToken = default)
        {
            object itemMap = _map.GetOrAdd(key, (_) => new ConcurrentDictionary<string, object>());

            ConcurrentDictionary<string, object> hash = (ConcurrentDictionary<string, object>)itemMap;

            for (int i = 0; i < itemKeys.Length; i++)
            {
                hash[itemKeys[i]] = items[i];
            }

            return Task.CompletedTask;
        }

        public async Task<bool> HashSet(string key, string itemKey, string item, CancellationToken cancellationToken = default)
        {
            return await HashSet<string>(key, itemKey, item, cancellationToken);
        }

        public async Task HashSet(string key, string[] itemKeys, string[] items, CancellationToken cancellationToken = default)
        {
            await HashSet<string>(key, itemKeys, items, cancellationToken);
        }

        public Task<bool> KeyExists(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_map.ContainsKey(key));
        }

        public Task<bool> RemoveKey(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_map.TryRemove(key, out _));
        }

        public Task<T> StringGet<T>(string key, CancellationToken cancellationToken = default)
        {
            T obj = default;

            if (_map.TryGetValue(key, out object item))
            {
                obj = (T)item;
            }

            return Task.FromResult(obj);
        }

        public async Task<string> StringGet(string key, CancellationToken cancellationToken = default)
        {
            return await StringGet<string>(key, cancellationToken);
        }

        public async Task<bool> StringSet(string key, string value, CancellationToken cancellationToken = default)
        {
            return await StringSet<string>(key, value, cancellationToken);
        }

        public Task<bool> StringSet<T>(string key, T value, CancellationToken cancellationToken = default)
        {
            _map[key] = value;

            return Task.FromResult(true);
        }
    }
}
