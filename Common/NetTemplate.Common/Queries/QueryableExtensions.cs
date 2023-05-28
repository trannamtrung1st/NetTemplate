using NetTemplate.Common.Expressions;
using System.Linq.Dynamic.Core;
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
            if (query.IsOrdered())
                query = (query as IOrderedQueryable<T>).ThenBy(property);
            else
                query = query.OrderBy(property);
            return query;
        }

        public static IQueryable<T> SequentialOrderByDesc<T>(this IQueryable<T> query,
            string property)
        {
            if (query.IsOrdered())
                query = (query as IOrderedQueryable<T>).ThenBy($"{property} DESC");
            else
                query = query.OrderBy($"{property} DESC");
            return query;
        }

        public static IQueryable<TEntity> SortSequential<TEntity>(this IQueryable<TEntity> query,
            string property, bool isDesc)
        {
            return isDesc
                ? query.SequentialOrderByDesc(property)
                : query.SequentialOrderBy(property);
        }

        // [NOTE] custom implementation
        private static IQueryable<TEntity> SortDynamic<TEntity>(this IQueryable<TEntity> query,
            string property, bool isDesc)
        {
            ExpressionHelper.BuildNavigationsFromPropertyString<TEntity>(property,
                out string[] _,
                out Expression body,
                out ParameterExpression param);

            if (body.Type == typeof(bool))
            {
                Expression<Func<TEntity, bool>> expr = ExpressionHelper.BuildPropertySelectorExpression<TEntity, bool>(body, param);

                return isDesc ? query.SequentialOrderByDesc(expr) : query.SequentialOrderBy(expr);
            }
            else if (body.Type == typeof(bool?))
            {
                Expression<Func<TEntity, bool?>> expr = ExpressionHelper.BuildPropertySelectorExpression<TEntity, bool?>(body, param);

                return isDesc ? query.SequentialOrderByDesc(expr) : query.SequentialOrderBy(expr);
            }
            else if (body.Type == typeof(decimal))
            {
                Expression<Func<TEntity, decimal>> expr = ExpressionHelper.BuildPropertySelectorExpression<TEntity, decimal>(body, param);

                return isDesc ? query.SequentialOrderByDesc(expr) : query.SequentialOrderBy(expr);
            }
            else if (body.Type == typeof(decimal?))
            {
                Expression<Func<TEntity, decimal?>> expr = ExpressionHelper.BuildPropertySelectorExpression<TEntity, decimal?>(body, param);

                return isDesc ? query.SequentialOrderByDesc(expr) : query.SequentialOrderBy(expr);
            }
            else if (body.Type == typeof(double))
            {
                Expression<Func<TEntity, double>> expr = ExpressionHelper.BuildPropertySelectorExpression<TEntity, double>(body, param);

                return isDesc ? query.SequentialOrderByDesc(expr) : query.SequentialOrderBy(expr);
            }
            else if (body.Type == typeof(double?))
            {
                Expression<Func<TEntity, double?>> expr = ExpressionHelper.BuildPropertySelectorExpression<TEntity, double?>(body, param);

                return isDesc ? query.SequentialOrderByDesc(expr) : query.SequentialOrderBy(expr);
            }
            else if (body.Type == typeof(float))
            {
                Expression<Func<TEntity, float>> expr = ExpressionHelper.BuildPropertySelectorExpression<TEntity, float>(body, param);

                return isDesc ? query.SequentialOrderByDesc(expr) : query.SequentialOrderBy(expr);
            }
            else if (body.Type == typeof(float?))
            {
                Expression<Func<TEntity, float?>> expr = ExpressionHelper.BuildPropertySelectorExpression<TEntity, float?>(body, param);

                return isDesc ? query.SequentialOrderByDesc(expr) : query.SequentialOrderBy(expr);
            }
            else if (body.Type == typeof(int))
            {
                Expression<Func<TEntity, int>> expr = ExpressionHelper.BuildPropertySelectorExpression<TEntity, int>(body, param);

                return isDesc ? query.SequentialOrderByDesc(expr) : query.SequentialOrderBy(expr);
            }
            else if (body.Type == typeof(int?))
            {
                Expression<Func<TEntity, int?>> expr = ExpressionHelper.BuildPropertySelectorExpression<TEntity, int?>(body, param);

                return isDesc ? query.SequentialOrderByDesc(expr) : query.SequentialOrderBy(expr);
            }
            else if (body.Type == typeof(long))
            {
                Expression<Func<TEntity, long>> expr = ExpressionHelper.BuildPropertySelectorExpression<TEntity, long>(body, param);

                return isDesc ? query.SequentialOrderByDesc(expr) : query.SequentialOrderBy(expr);
            }
            else if (body.Type == typeof(long?))
            {
                Expression<Func<TEntity, long?>> expr = ExpressionHelper.BuildPropertySelectorExpression<TEntity, long?>(body, param);

                return isDesc ? query.SequentialOrderByDesc(expr) : query.SequentialOrderBy(expr);
            }
            else
            {
                Expression<Func<TEntity, object>> expr = ExpressionHelper.BuildPropertySelectorExpression<TEntity, object>(body, param);

                return isDesc ? query.SequentialOrderByDesc(expr) : query.SequentialOrderBy(expr);
            }
        }
    }
}
