using NetTemplate.Blog.ApplicationCore.Post.Models;
using NetTemplate.Shared.ApplicationCore.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Commands.UpdatePost
{
    public class UpdatePostCommand : ITransactionalCommand
    {
        public UpdatePostModel Model { get; }

        public UpdatePostCommand(UpdatePostModel model)
        {
            Model = model;
        }
    }
}
