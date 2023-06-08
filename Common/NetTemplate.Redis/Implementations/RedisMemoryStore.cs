using NetTemplate.Common.MemoryStore.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace NetTemplate.Redis.Implementations
{
    public class RedisMemoryStore : IMemoryStore
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public RedisMemoryStore(ConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<T> HashGet<T>(string key, string itemKey, CancellationToken cancellationToken = default)
        {
            T obj = default;

            string value = await HashGet(key, itemKey, cancellationToken);

            if (value != null)
            {
                obj = JsonConvert.DeserializeObject<T>(value);
            }

            return obj;
        }

        public async Task<string> HashGet(string key, string itemKey, CancellationToken cancellationToken = default)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();

            RedisValue value = await db.HashGetAsync(key, itemKey);

            return value;
        }

        public async Task<T[]> HashGetAll<T>(string key, string[] exceptKeys = null, CancellationToken cancellationToken = default)
        {
            T[] items = default;

            IDatabase db = _connectionMultiplexer.GetDatabase();

            IEnumerable<HashEntry> entries = await db.HashGetAllAsync(key);

            if (entries.Any())
            {
                if (exceptKeys?.Length > 0)
                {
                    entries = entries.Where(e => !exceptKeys.Contains((string)e.Name));
                }

                items = entries.Select(value => JsonConvert.DeserializeObject<T>(value.Value)).ToArray();
            }

            return items;
        }

        public async Task<string[]> HashGetAll(string key, string[] exceptKeys = null, CancellationToken cancellationToken = default)
        {
            string[] items = default;

            IDatabase db = _connectionMultiplexer.GetDatabase();

            IEnumerable<HashEntry> entries = await db.HashGetAllAsync(key);

            if (entries?.Any() == true)
            {
                if (exceptKeys?.Length > 0)
                {
                    entries = entries.Where(e => !exceptKeys.Contains((string)e.Name));
                }

                items = entries.Select(entry => (string)entry.Value).ToArray();
            }

            return items;
        }

        public async Task<bool> HashRemove(string key, string itemKey, CancellationToken cancellationToken = default)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();

            return await db.HashDeleteAsync(key, itemKey);
        }

        public async Task<bool> HashSet<T>(string key, string itemKey, T item, CancellationToken cancellationToken = default)
        {
            return await HashSet(key, itemKey, JsonConvert.SerializeObject(item), cancellationToken);
        }

        public async Task HashSet<T>(string key, string[] itemKeys, T[] items, CancellationToken cancellationToken = default)
        {
            string[] values = items.Select(item => JsonConvert.SerializeObject(item)).ToArray();

            await HashSet(key, itemKeys, values, cancellationToken);
        }

        public async Task<bool> HashSet(string key, string itemKey, string item, CancellationToken cancellationToken = default)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();

            return await db.HashSetAsync(key, itemKey, item);
        }

        public async Task HashSet(string key, string[] itemKeys, string[] items, CancellationToken cancellationToken = default)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();

            HashEntry[] hashEntries = itemKeys.Zip(items)
                .Select(entry => new HashEntry(entry.First, entry.Second))
                .ToArray();

            await db.HashSetAsync(key, hashEntries);
        }

        public async Task<bool> KeyExists(string key, CancellationToken cancellationToken = default)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();

            return await db.KeyExistsAsync(key);
        }

        public async Task<bool> RemoveKey(string key, CancellationToken cancellationToken = default)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();

            return await db.KeyDeleteAsync(key);
        }

        public async Task<T> Get<T>(string key, CancellationToken cancellationToken = default)
        {
            T obj = default;

            string value = await Get(key, cancellationToken);

            if (value != null)
            {
                obj = JsonConvert.DeserializeObject<T>(value);
            }

            return obj;
        }

        public async Task<string> Get(string key, CancellationToken cancellationToken = default)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();

            RedisValue value = await db.StringGetAsync(key);

            return value;
        }

        public async Task<bool> Set(string key, string value, CancellationToken cancellationToken = default)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();

            return await db.StringSetAsync(key, value);
        }

        public async Task<bool> Set<T>(string key, T value, CancellationToken cancellationToken = default)
        {
            return await Set(key, JsonConvert.SerializeObject(value), cancellationToken);
        }
    }
}
