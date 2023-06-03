namespace NetTemplate.Shared.ApplicationCore.Common.Interfaces
{
    public interface IEntityCache<T>
    {
        Task<T> GetEntryOrAdd(string entityKey, string currentVersion, Func<CancellationToken, Task<T>> createFunc, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
        Task<bool> UpdateEntry(string entityKey, T entry, string currentVersion, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
        Task<bool> RemoveEntry(string entityKey, CancellationToken cancellationToken = default);
    }
}
