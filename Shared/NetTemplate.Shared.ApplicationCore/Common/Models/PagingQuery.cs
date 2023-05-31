namespace NetTemplate.Shared.ApplicationCore.Common.Models
{
    public interface IPagingQuery
    {
        int? Take { get; set; }
        bool CanGetAll();
        int GetTakeOrDefault();
    }

    public interface IOffsetPagingQuery : IPagingQuery
    {
        int Skip { get; set; }
    }

    public interface IKeySetPagingQuery<TData> : IPagingQuery where TData : struct
    {
        TData? KeyAfter { get; set; }
        TData? KeyBefore { get; set; }
    }
}
