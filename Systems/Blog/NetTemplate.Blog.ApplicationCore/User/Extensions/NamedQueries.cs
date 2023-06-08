using NetTemplate.Blog.ApplicationCore.User;

namespace System.Linq
{
    public static partial class NamedQueries
    {
        public static IQueryable<UserPartialEntity> ByCode(this IQueryable<UserPartialEntity> query, string code)
        {
            return query.Where(e => e.UserCode == code);
        }
    }
}
