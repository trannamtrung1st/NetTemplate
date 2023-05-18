using System.Linq.Expressions;

namespace System.Linq
{
    public static class QueryableExtensions
    {
        public static bool IsOrdered<T>(this IQueryable<T> query)
        {
            return query.Expression.Type == typeof(IOrderedQueryable<T>);
        }

        public static IQueryable<T> SequentialOrderBy<T, TKey>(this IQueryable<T> query,
            Expression<Func<T, TKey>> keySelector)
        {
            if (query.IsOrdered())
                query = (query as IOrderedQueryable<T>).ThenBy(keySelector);
            else
                query = query.OrderBy(keySelector);

            return query;
        }

        public static IQueryable<T> SequentialOrderByDesc<T, TKey>(this IQueryable<T> query,
            Expression<Func<T, TKey>> keySelector)
        {
            if (query.IsOrdered())
                query = (query as IOrderedQueryable<T>).ThenByDescending(keySelector);
            else
                query = query.OrderByDescending(keySelector);
            return query;
        }

        public static IQueryable<TEntity> SortSequential<TEntity, TProperty>(this IQueryable<TEntity> query,
            Expression<Func<TEntity, TProperty>> expr, bool isDesc)
        {
            return isDesc
                ? query.SequentialOrderByDesc(expr)
                : query.SequentialOrderBy(expr);
        }
    }
}
