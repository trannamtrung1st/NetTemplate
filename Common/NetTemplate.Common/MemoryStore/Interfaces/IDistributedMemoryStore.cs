namespace NetTemplate.Common.MemoryStore.Interfaces
{
    public interface IDistributedMemoryStore
    {
        Task<bool> KeyExists(string key, CancellationToken cancellationToken = default);
        Task<bool> RemoveKey(string key, CancellationToken cancellationToken = default);
        Task<bool> Set(string key, string value, CancellationToken cancellationToken = default);
        Task<bool> Set<T>(string key, T value, CancellationToken cancellationToken = default);
        Task<string> Get(string key, CancellationToken cancellationToken = default);
        Task<T> Get<T>(string key, CancellationToken cancellationToken = default);
        Task<string[]> HashGetAll(string key, string[] exceptKeys = null, CancellationToken cancellationToken = default);
        Task<T[]> HashGetAll<T>(string key, string[] exceptKeys = null, CancellationToken cancellationToken = default);
        Task<string> HashGet(string key, string itemKey, CancellationToken cancellationToken = default);
        Task<T> HashGet<T>(string key, string itemKey, CancellationToken cancellationToken = default);
        Task<bool> HashSet(string key, string itemKey, string item, CancellationToken cancellationToken = default);
        Task<bool> HashSet<T>(string key, string itemKey, T item, CancellationToken cancellationToken = default);
        Task HashSet(string key, string[] itemKeys, string[] items, CancellationToken cancellationToken = default);
        Task HashSet<T>(string key, string[] itemKeys, T[] items, CancellationToken cancellationToken = default);
        Task<bool> HashRemove(string key, string itemKey, CancellationToken cancellationToken = default);
    }
}
