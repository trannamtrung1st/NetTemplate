using System.Linq.Expressions;

namespace NetTemplate.Common.Expressions
{
    public static class TypeExtensions
    {
        public static Expression<Func<TEntity, TProperty>> BuildPropertySelectorExpression<TEntity, TProperty>(this Type type, string property)
        {
            ParameterExpression param = Expression.Parameter(typeof(TEntity));
            MemberExpression body = Expression.Property(param, property);
            return Expression.Lambda<Func<TEntity, TProperty>>(body, param);
        }
    }
}
