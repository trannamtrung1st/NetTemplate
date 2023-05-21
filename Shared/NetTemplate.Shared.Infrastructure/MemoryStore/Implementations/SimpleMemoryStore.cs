using NetTemplate.Shared.ApplicationCore.MemoryStore.Interfaces;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace NetTemplate.Shared.Infrastructure.MemoryStore.Implementations
{
    public class SimpleMemoryStore : IMemoryStore
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _map;

        public SimpleMemoryStore()
        {
            _map = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();
        }

        public Task<T> HashGet<T>(string key, string itemKey, CancellationToken cancellationToken = default)
        {
            T obj = default;

            if (_map.TryGetValue(key, out ConcurrentDictionary<string, string> item))
            {
                if (item.TryGetValue(itemKey, out string value))
                {
                    obj = JsonConvert.DeserializeObject<T>(value);
                }
            }

            return Task.FromResult(obj);
        }

        public Task<T[]> HashGetAll<T>(string key, CancellationToken cancellationToken = default)
        {
            T[] list = default;

            if (_map.TryGetValue(key, out ConcurrentDictionary<string, string> item))
            {
                list = item.Values.Select(v => JsonConvert.DeserializeObject<T>(v)).ToArray();
            }

            return Task.FromResult(list);
        }

        public Task<bool> HashRemove(string key, string itemKey, CancellationToken cancellationToken = default)
        {
            if (_map.TryGetValue(key, out ConcurrentDictionary<string, string> item))
            {
                if (item.TryRemove(itemKey, out _))
                {
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }

        public Task<bool> HashSet<T>(string key, string itemKey, T item, CancellationToken cancellationToken = default)
        {
            if (_map.TryGetValue(key, out ConcurrentDictionary<string, string> itemMap))
            {
                itemMap[itemKey] = JsonConvert.SerializeObject(item);

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task HashSet<T>(string key, string[] itemKeys, T[] items, CancellationToken cancellationToken = default)
        {
            if (_map.TryGetValue(key, out ConcurrentDictionary<string, string> itemMap))
            {
                for (int i = 0; i < itemKeys.Length; i++)
                {
                    itemMap[itemKeys[i]] = JsonConvert.SerializeObject(items[i]);
                }
            }

            return Task.CompletedTask;
        }

        public Task<bool> KeyExists(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_map.ContainsKey(key));
        }

        public Task<bool> RemoveHash(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_map.TryRemove(key, out _));
        }
    }
}
