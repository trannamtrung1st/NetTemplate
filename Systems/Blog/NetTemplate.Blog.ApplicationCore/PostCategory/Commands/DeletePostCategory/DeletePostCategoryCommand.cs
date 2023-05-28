using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Commands.DeletePostCategory
{
    public class DeletePostCategoryCommand : ITransactionalCommand
    {
        public int Id { get; }

        public DeletePostCategoryCommand(int id)
        {
            Id = id;
        }
    }
}
