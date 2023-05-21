namespace NetTemplate.Shared.ApplicationCore.Common.Models
{
    public interface ISortableQuery<T>
    {
        T SortBy { get; set; }
        bool IsDesc { get; set; }
    }
}
