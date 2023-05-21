using Microsoft.EntityFrameworkCore.Metadata;
using NetTemplate.Blog.Infrastructure.Persistence.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Entities;
using System.Linq.Expressions;

namespace NetTemplate.Blog.Infrastructure.Persistence.QueryFilters
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
