namespace NetTemplate.Blog.ApplicationCore.PostCategory.Interfaces
{
    public interface IPostCategoryValidator
    {
        Task ValidatePostCategoryName(string currentName, string newName, CancellationToken cancellationToken = default);
    }
}
