namespace NetTemplate.Blog.ApplicationCore.Cross.Interfaces
{
    public interface IEntityVersionManager
    {
        Task<string> UpdateVersion(string entityName, string key);
        Task<string> GetVersion(string entityName, string key);
        Task Remove(string entityName, string key);
    }
}
