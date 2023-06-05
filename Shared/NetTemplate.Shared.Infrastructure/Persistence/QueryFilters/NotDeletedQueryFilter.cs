using Microsoft.EntityFrameworkCore.Metadata;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using NetTemplate.Shared.Infrastructure.Persistence.Interfaces;
using System.Linq.Expressions;

namespace NetTemplate.Shared.Infrastructure.Persistence.QueryFilters
{
    public class NotDeletedQueryFilter : IQueryFilterProvider
    {
        public string ProvideMethodName => nameof(CreateFilter);

        public bool CanApply(IMutableEntityType eType)
            => typeof(ISoftDeleteEntity).IsAssignableFrom(eType.ClrType);

        public Expression<Func<TEntity, bool>> CreateFilter<TEntity>() where TEntity : ISoftDeleteEntity
            => (o) => !o.IsDeleted;
    }
}
