using NetTemplate.Common.Expressions;
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

        public static IQueryable<T> SequentialOrderBy<T>(this IQueryable<T> query,
            string property)
        {
            Expression<Func<T, object>> expr = typeof(T).BuildPropertySelectorExpression<T, object>(property);

            return query.SequentialOrderBy(expr);
        }

        public static IQueryable<T> SequentialOrderByDesc<T>(this IQueryable<T> query,
            string property)
        {
            Expression<Func<T, object>> expr = typeof(T).BuildPropertySelectorExpression<T, object>(property);

            return query.SequentialOrderByDesc(expr);
        }

        public static IQueryable<TEntity> SortSequential<TEntity>(this IQueryable<TEntity> query,
            string property, bool isDesc)
        {
            return isDesc
                ? query.SequentialOrderByDesc(property)
                : query.SequentialOrderBy(property);
        }
    }
}
