namespace NetTemplate.Blog.ApplicationCore.Cross.Interfaces
{
    public interface IEntityVersionManager
    {
        Task<string> UpdateVersion(string entityName, string key, CancellationToken cancellationToken = default);
        Task<string> GetVersion(string entityName, string key, CancellationToken cancellationToken = default);
        Task Remove(string entityName, string key, CancellationToken cancellationToken = default);
    }
}
