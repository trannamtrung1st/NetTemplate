using Microsoft.EntityFrameworkCore.Metadata;

namespace NetTemplate.Blog.Infrastructure.Persistence.Interfaces
{
    public interface IQueryFilterProvider
    {
        string ProvideMethodName { get; }
        bool CanApply(IMutableEntityType eType);
    }
}
