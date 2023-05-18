using NetTemplate.Shared.ApplicationCore.Entities;

namespace NetTemplate.Shared.ApplicationCore.Extensions
{
    public static class NamedQueries
    {
        public static IQueryable<T> IsDeleted<T>(this IQueryable<T> query) where T : ISoftDeleteEntity
        {
            return query.Where(e => e.IsDeleted);
        }

        public static IEnumerable<T> IsDeleted<T>(this IEnumerable<T> query) where T : ISoftDeleteEntity
        {
            return query.Where(e => e.IsDeleted);
        }

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
