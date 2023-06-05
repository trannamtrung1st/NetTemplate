namespace NetTemplate.Shared.ApplicationCore.Common.Interfaces
{
    public interface ISortableQuery<TSortBy>
    {
        TSortBy[] SortBy { get; set; }
        bool[] IsDesc { get; set; }
    }
}
