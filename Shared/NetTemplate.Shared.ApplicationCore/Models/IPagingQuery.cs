namespace NetTemplate.Shared.ApplicationCore.Models
{
    public interface IPagingQuery
    {
        int Skip { get; set; }
        int? Take { get; set; }
        bool CanGetAll();
    }
}
