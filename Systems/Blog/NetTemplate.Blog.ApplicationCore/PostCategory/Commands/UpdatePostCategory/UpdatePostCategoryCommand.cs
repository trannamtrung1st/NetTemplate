using NetTemplate.Blog.ApplicationCore.PostCategory.Models;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Commands.UpdatePostCategory
{
    public class UpdatePostCategoryCommand : ITransactionalCommand
    {
        public int Id { get; }
        public UpdatePostCategoryModel Model { get; }

        public UpdatePostCategoryCommand(int id, UpdatePostCategoryModel model)
        {
            Id = id;
            Model = model;
        }
    }
}
