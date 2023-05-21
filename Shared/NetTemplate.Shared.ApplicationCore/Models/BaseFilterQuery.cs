namespace NetTemplate.Shared.ApplicationCore.Models
{
    public abstract class BaseFilterQuery : IPagingQuery, ISearchQuery
    {
        public string Terms { get; set; }
        public int Skip { get; set; }
        public int? Take { get; set; }

        public abstract bool CanGetAll();
    }
}
