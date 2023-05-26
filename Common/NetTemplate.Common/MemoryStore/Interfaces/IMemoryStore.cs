namespace NetTemplate.Common.MemoryStore.Interfaces
{
    public interface IMemoryStore
    {
        Task<T[]> HashGetAll<T>(string key, CancellationToken cancellationToken = default);
        Task<T> HashGet<T>(string key, string itemKey, CancellationToken cancellationToken = default);
        Task<bool> KeyExists(string key, CancellationToken cancellationToken = default);
        Task<bool> HashSet<T>(string key, string itemKey, T item, CancellationToken cancellationToken = default);
        Task HashSet<T>(string key, string[] itemKeys, T[] items, CancellationToken cancellationToken = default);
        Task<bool> HashRemove(string key, string itemKey, CancellationToken cancellationToken = default);
        Task<bool> RemoveHash(string key, CancellationToken cancellationToken = default);
    }
}
