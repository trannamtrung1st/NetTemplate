using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.Post.Commands.DeletePost
{
    public class DeletePostCommand : ITransactionalCommand
    {
        public int Id { get; }

        public DeletePostCommand(int id)
        {
            Id = id;
        }
    }
}
