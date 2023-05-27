namespace NetTemplate.Shared.ApplicationCore.Common.Models
{
    public interface ISortableQuery<TSortBy>
    {
        TSortBy[] SortBy { get; set; }
        bool[] IsDesc { get; set; }
    }
}
