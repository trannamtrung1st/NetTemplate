using NetTemplate.Blog.ApplicationCore.Comment.Models;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.Comment.Commands.CreatePostComment
{
    public class CreatePostCommentCommand : ITransactionalCommand
    {
        public int OnPostId { get; }
        public CreateCommentModel Model { get; }

        public CreatePostCommentCommand(int onPostId, CreateCommentModel model)
        {
            OnPostId = onPostId;
            Model = model;
        }
    }
}
