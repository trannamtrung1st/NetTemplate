using FilterDefaults = NetTemplate.Shared.ApplicationCore.Common.Constants.Filter;

namespace NetTemplate.Shared.ApplicationCore.Common.Models
{
    public abstract class BaseFilterQuery : IPagingQuery, ISearchQuery
    {
        public string Terms { get; set; }
        public IEnumerable<int> Ids { get; set; }
        public int Skip { get; set; }
        public int? Take { get; set; }

        public virtual int GetTakeOrDefault() => Take ?? FilterDefaults.Take;

        public abstract bool CanGetAll();
    }
}
