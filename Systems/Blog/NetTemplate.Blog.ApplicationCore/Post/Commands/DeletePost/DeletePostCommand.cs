using NetTemplate.Shared.ApplicationCore.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Commands.DeletePost
{
    public class DeletePostCommand : ITransactionalCommand
    {
        public int PostId { get; }

        public DeletePostCommand(int postId)
        {
            PostId = postId;
        }
    }
}
