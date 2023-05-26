using Microsoft.EntityFrameworkCore.Metadata;

namespace NetTemplate.Shared.Infrastructure.Persistence.Interfaces
{
    public interface IQueryFilterProvider
    {
        string ProvideMethodName { get; }
        bool CanApply(IMutableEntityType eType);
    }
}
