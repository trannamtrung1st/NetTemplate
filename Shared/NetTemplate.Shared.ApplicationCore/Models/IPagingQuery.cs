namespace NetTemplate.Shared.ApplicationCore.Models
{
    public interface IPagingQuery
    {
        int Skip { get; }
        int? Take { get; }
        bool CanGetAll();
    }
}
