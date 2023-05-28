using NetTemplate.Common.MemoryStore.Interfaces;
using Newtonsoft.Json;
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
                ConcurrentDictionary<string, string> hash = (ConcurrentDictionary<string, string>)item;

                if (hash.TryGetValue(itemKey, out string value))
                {
                    obj = JsonConvert.DeserializeObject<T>(value);
                }
            }

            return Task.FromResult(obj);
        }

        public Task<T[]> HashGetAll<T>(string key, CancellationToken cancellationToken = default)
        {
            T[] list = default;

            if (_map.TryGetValue(key, out object item))
            {
                ConcurrentDictionary<string, string> hash = (ConcurrentDictionary<string, string>)item;

                list = hash.Values.Select(v => JsonConvert.DeserializeObject<T>(v)).ToArray();
            }

            return Task.FromResult(list);
        }

        public Task<bool> HashRemove(string key, string itemKey, CancellationToken cancellationToken = default)
        {
            if (_map.TryGetValue(key, out object item))
            {
                ConcurrentDictionary<string, string> hash = (ConcurrentDictionary<string, string>)item;

                if (hash.TryRemove(itemKey, out _))
                {
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }

        public Task<bool> HashSet<T>(string key, string itemKey, T item, CancellationToken cancellationToken = default)
        {
            object itemMap = _map.GetOrAdd(key, (_) => new ConcurrentDictionary<string, string>());

            ConcurrentDictionary<string, string> hash = (ConcurrentDictionary<string, string>)itemMap;

            hash[itemKey] = JsonConvert.SerializeObject(item);

            return Task.FromResult(true);
        }

        public Task HashSet<T>(string key, string[] itemKeys, T[] items, CancellationToken cancellationToken = default)
        {
            object itemMap = _map.GetOrAdd(key, (_) => new ConcurrentDictionary<string, string>());

            ConcurrentDictionary<string, string> hash = (ConcurrentDictionary<string, string>)itemMap;

            for (int i = 0; i < itemKeys.Length; i++)
            {
                hash[itemKeys[i]] = JsonConvert.SerializeObject(items[i]);
            }

            return Task.CompletedTask;
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

        public Task<bool> StringSet(string key, string value, CancellationToken cancellationToken = default)
        {
            _map[key] = value;

            return Task.FromResult(true);
        }
    }
}
