namespace NetTemplate.Shared.ApplicationCore.Common.Models
{
    public abstract class BaseFilterQuery : BasePagingQuery, IOffsetPagingQuery, ISearchQuery
    {
        public string Terms { get; set; }
        public int Skip { get; set; }
    }
}
