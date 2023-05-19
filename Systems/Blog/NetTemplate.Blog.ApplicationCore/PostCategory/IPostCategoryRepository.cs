using NetTemplate.Shared.ApplicationCore.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.PostCategory
{
    public interface IPostCategoryRepository : ISoftDeleteRepository<PostCategoryEntity>
    {
    }
}
