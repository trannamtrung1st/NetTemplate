using FilterDefaults = NetTemplate.Shared.ApplicationCore.Common.Constants.Filter;

namespace NetTemplate.Shared.ApplicationCore.Common.Models
{
    public abstract class BasePagingQuery : IPagingQuery
    {
        public int? Take { get; set; }
        public virtual int GetTakeOrDefault() => Take ?? FilterDefaults.Take;

        public abstract bool CanGetAll();
    }
}
