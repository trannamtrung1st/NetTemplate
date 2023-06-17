using System.Linq.Expressions;

namespace NetTemplate.Common.Expressions.Utils
{
    public static class ExpressionHelper
    {
        public static void BuildNavigationsFromPropertyString<TEntity>(string property,
            out string[] navigations, out Expression body, out ParameterExpression param)
        {
            navigations = property.Split('.');

            if (navigations.Length == 0) throw new ArgumentException(null, nameof(property));

            Type entityType = typeof(TEntity);

            param = Expression.Parameter(entityType);

            body = param;

            foreach (string navigation in navigations)
            {
                body = Expression.Property(body, navigation);
            }
        }

        public static Expression<Func<TEntity, TProperty>> BuildPropertySelectorExpression<TEntity, TProperty>(
            Expression body, ParameterExpression param)
        {
            return Expression.Lambda<Func<TEntity, TProperty>>(body, param);
        }
    }
}
