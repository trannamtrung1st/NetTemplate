using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Shared.ApplicationCore.Common.Models
{
    public abstract class BasePagingQuery : IPagingQuery
    {
        public int? Take { get; set; }
        public virtual int GetTakeOrDefault() => Take ?? TakeDefault;

        public abstract bool CanGetAll();

        public const int TakeDefault = 10;
    }
}
