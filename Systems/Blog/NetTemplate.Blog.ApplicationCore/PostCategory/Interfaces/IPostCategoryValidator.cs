namespace NetTemplate.Blog.ApplicationCore.PostCategory.Interfaces
{
    public interface IPostCategoryValidator
    {
        Task ValidatePostCategoryName(string name, CancellationToken cancellationToken = default);
    }
}
