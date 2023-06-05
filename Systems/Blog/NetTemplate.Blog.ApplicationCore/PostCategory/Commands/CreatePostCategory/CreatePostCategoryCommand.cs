using NetTemplate.Blog.ApplicationCore.PostCategory.Models;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Commands.CreatePostCategory
{
    public class CreatePostCategoryCommand : ITransactionalCommand
    {
        public CreatePostCategoryModel Model { get; }

        public CreatePostCategoryCommand(CreatePostCategoryModel model)
        {
            Model = model;
        }
    }
}
