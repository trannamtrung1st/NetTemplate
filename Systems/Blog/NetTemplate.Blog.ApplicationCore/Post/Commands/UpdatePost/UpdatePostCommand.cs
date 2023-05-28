using NetTemplate.Blog.ApplicationCore.Post.Models;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Commands.UpdatePost
{
    public class UpdatePostCommand : ITransactionalCommand
    {
        public int Id { get; }
        public UpdatePostModel Model { get; }

        public UpdatePostCommand(int id, UpdatePostModel model)
        {
            Id = id;
            Model = model;
        }
    }
}
