namespace NetTemplate.Shared.ApplicationCore.Models
{
    public interface ISortableQuery<T>
    {
        T SortBy { get; }
        bool IsDesc { get; }
    }
}
