using NetTemplate.Shared.ApplicationCore.Common.Entities;

namespace NetTemplate.Shared.ApplicationCore.Common.Extensions
{
    public static class NamedQueries
    {
        public static IQueryable<T> IsNotDeleted<T>(this IQueryable<T> query) where T : ISoftDeleteEntity
        {
            return query.Where(e => !e.IsDeleted);
        }

        public static IEnumerable<T> IsNotDeleted<T>(this IEnumerable<T> query) where T : ISoftDeleteEntity
        {
            return query.Where(e => !e.IsDeleted);
        }
    }
}
