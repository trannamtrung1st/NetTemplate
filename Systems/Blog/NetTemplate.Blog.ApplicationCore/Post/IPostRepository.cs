using NetTemplate.Shared.ApplicationCore.Interfaces.Repositories;

namespace NetTemplate.Blog.ApplicationCore.Post
{
    public interface IPostRepository : ISoftDeleteRepository<PostEntity>
    {
    }
}
