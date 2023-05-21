using NetTemplate.Blog.ApplicationCore.Post.Models;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Commands.CreatePost
{
    public class CreatePostCommand : ITransactionalCommand
    {
        public CreatePostModel Model { get; }

        public CreatePostCommand(CreatePostModel model)
        {
            Model = model;
        }
    }
}
