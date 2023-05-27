using NetTemplate.Shared.ApplicationCore.Common.Entities;

namespace System.Linq
{
    public static class NamedQueries
    {
        public static IQueryable<T> IsNotDeleted<T>(this IQueryable<T> query) where T : ISoftDeleteEntity
        {
            return query.Where(e => !e.IsDeleted);
        }

        public static IQueryable<T> ById<T, TId>(this IQueryable<T> query, TId id)
            where T : IHasId<TId>
        {
            return query.Where(e => Equals(e.Id, id));
        }

        public static IQueryable<T> ByIds<T, TId>(this IQueryable<T> query, IEnumerable<TId> ids)
            where T : IHasId<TId>
        {
            return query.Where(e => ids.Contains(e.Id));
        }
    }
}
