namespace NetTemplate.Shared.ApplicationCore.Models
{
    public abstract class BaseFilterQuery : IPagingQuery, ISearchQuery
    {
        public BaseFilterQuery() { }
        public BaseFilterQuery(string terms,
            int skip, int? take)
        {
            Terms = terms;
            Skip = skip;
            Take = take;
        }

        public string Terms { get; }
        public int Skip { get; }
        public int? Take { get; }

        public abstract bool CanGetAll();
    }
}
