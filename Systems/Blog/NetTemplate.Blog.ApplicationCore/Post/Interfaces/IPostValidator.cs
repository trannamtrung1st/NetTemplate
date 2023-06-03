namespace NetTemplate.Blog.ApplicationCore.Post.Interfaces
{
    public interface IPostValidator
    {
        Task ValidatePostTitle(string title, CancellationToken cancellationToken = default);
        Task ValidateExistences(int categoryId, CancellationToken cancellationToken = default);
    }
}
