namespace NetTemplate.Shared.ApplicationCore.Models
{
    public abstract class BaseSortableFilterQuery<TSortBy> : BaseFilterQuery, ISortableQuery<TSortBy>
    {
        public TSortBy SortBy { get; set; }
        public bool IsDesc { get; set; }
    }
}
