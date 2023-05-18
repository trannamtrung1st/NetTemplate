using NetTemplate.Shared.ApplicationCore.Interfaces.Repositories;

namespace NetTemplate.Blog.ApplicationCore.PostCategory
{
    public interface IPostCategoryRepository : ISoftDeleteRepository<PostCategoryEntity>
    {
    }
}
