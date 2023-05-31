using NetTemplate.Blog.ApplicationCore.Comment;

namespace System.Linq
{
    public static partial class NamedQueries
    {
        public static IQueryable<CommentEntity> JoinRequiredRelationships(this IQueryable<CommentEntity> query)
        {
            return query.Where(e => e.OnPost.Id > 0);
        }

        public static IQueryable<CommentEntity> OnPost(this IQueryable<CommentEntity> query, int onPostId)
        {
            return query.Where(e => e.OnPostId == onPostId);
        }
    }
}
