namespace NetTemplate.Shared.ApplicationCore.Models
{
    public abstract class BaseSortableFilterQuery<T> : BaseFilterQuery, ISortableQuery<T>
    {
        public BaseSortableFilterQuery() { }
        public BaseSortableFilterQuery(T sortBy, bool isDesc, string terms,
            int skip, int? take) : base(terms, skip, take)
        {
            SortBy = sortBy;
            IsDesc = isDesc;
        }

        public T SortBy { get; }
        public bool IsDesc { get; }
    }
}
