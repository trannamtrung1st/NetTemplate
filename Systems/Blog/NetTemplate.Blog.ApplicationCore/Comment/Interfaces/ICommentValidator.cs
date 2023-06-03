namespace NetTemplate.Blog.ApplicationCore.Comment.Interfaces
{
    public interface ICommentValidator
    {
        Task ValidateExistences(int postId, CancellationToken cancellationToken = default);
    }
}
