namespace NetTemplate.Shared.ApplicationCore.Models
{
    public interface ISortableQuery<T>
    {
        T SortBy { get; set; }
        bool IsDesc { get; set; }
    }
}
