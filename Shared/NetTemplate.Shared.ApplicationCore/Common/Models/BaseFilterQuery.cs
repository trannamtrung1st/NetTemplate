using NetTemplate.Shared.ApplicationCore.Common.Constants;

namespace NetTemplate.Shared.ApplicationCore.Common.Models
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
