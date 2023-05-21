using NetTemplate.Shared.ApplicationCore.Constants;

namespace NetTemplate.Shared.ApplicationCore.Models
{
    public abstract class BaseFilterQuery : IPagingQuery, ISearchQuery
    {
        public string Terms { get; set; }
        public int Skip { get; set; }
        public int? Take { get; set; }

        public virtual int GetTakeOrDefault() => Take ?? FilterDefaults.DefaultTake;

        public abstract bool CanGetAll();
    }
}
