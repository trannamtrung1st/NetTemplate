using NetTemplate.Blog.ApplicationCore.Comment.Models;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.Comment.Commands.UpdateComment
{
    public class UpdateCommentCommand : ITransactionalCommand
    {
        public int Id { get; }
        public UpdateCommentModel Model { get; }

        public UpdateCommentCommand(int id, UpdateCommentModel model)
        {
            Id = id;
            Model = model;
        }
    }
}
