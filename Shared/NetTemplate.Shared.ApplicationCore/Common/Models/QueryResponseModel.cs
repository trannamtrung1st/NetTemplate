namespace NetTemplate.Shared.ApplicationCore.Common.Models
{
    public class QueryResponseModel<T>
    {
        public QueryResponseModel(int? total, IQueryable<T> query)
        {
            Total = total;
            Query = query;
        }

        public int? Total { get; set; }
        public IQueryable<T> Query { get; set; }
    }
}
