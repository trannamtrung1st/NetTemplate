namespace NetTemplate.Blog.ApplicationCore.Post.Interfaces
{
    public interface IPostValidator
    {
        Task ValidatePostTitle(string currentTitle, string newTitle, CancellationToken cancellationToken = default);
        Task ValidateExistences(int categoryId, CancellationToken cancellationToken = default);
    }
}
