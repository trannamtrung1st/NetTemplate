using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.Comment.Commands.DeleteComment
{
    public class DeleteCommentCommand : ITransactionalCommand
    {
        public int Id { get; }

        public DeleteCommentCommand(int id)
        {
            Id = id;
        }
    }
}
