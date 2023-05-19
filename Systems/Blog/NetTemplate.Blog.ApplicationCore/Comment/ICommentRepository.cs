using NetTemplate.Shared.ApplicationCore.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.Comment
{
    public interface ICommentRepository : ISoftDeleteRepository<CommentEntity>
    {
    }
}
