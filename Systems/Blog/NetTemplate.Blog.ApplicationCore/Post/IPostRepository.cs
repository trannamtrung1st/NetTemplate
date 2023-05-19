using NetTemplate.Shared.ApplicationCore.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.Post
{
    public interface IPostRepository : ISoftDeleteRepository<PostEntity>
    {
    }
}
