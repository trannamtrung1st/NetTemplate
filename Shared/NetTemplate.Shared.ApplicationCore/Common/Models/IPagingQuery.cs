namespace NetTemplate.Shared.ApplicationCore.Common.Models
{
    public interface IPagingQuery
    {
        int Skip { get; set; }
        int? Take { get; set; }
        bool CanGetAll();
        int GetTakeOrDefault();
    }
}
