using NetTemplate.Blog.ApplicationCore.Post;

namespace System.Linq
{
    public static partial class NamedQueries
    {
        public static IQueryable<PostEntity> JoinRequiredRelationships(this IQueryable<PostEntity> query)
        {
            return query.Where(e => e.Category.Id > 0);
        }
    }
}
