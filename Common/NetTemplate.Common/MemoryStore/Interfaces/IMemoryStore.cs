namespace NetTemplate.Common.MemoryStore.Interfaces
{
    public interface IMemoryStore
    {
        Task<bool> KeyExists(string key, CancellationToken cancellationToken = default);
        Task<bool> RemoveKey(string key, CancellationToken cancellationToken = default);
        Task<bool> StringSet(string key, string value, CancellationToken cancellationToken = default);
        Task<T> StringGet<T>(string key, CancellationToken cancellationToken = default);
        Task<T[]> HashGetAll<T>(string key, string[] exceptKeys = null, CancellationToken cancellationToken = default);
        Task<T> HashGet<T>(string key, string itemKey, CancellationToken cancellationToken = default);
        Task<bool> HashSet<T>(string key, string itemKey, T item, CancellationToken cancellationToken = default);
        Task HashSet<T>(string key, string[] itemKeys, T[] items, CancellationToken cancellationToken = default);
        Task<bool> HashRemove(string key, string itemKey, CancellationToken cancellationToken = default);
    }
}
