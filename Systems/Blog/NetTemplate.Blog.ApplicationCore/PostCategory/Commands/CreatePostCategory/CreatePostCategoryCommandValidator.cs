using FluentValidation;
using NetTemplate.Blog.ApplicationCore.PostCategory.Models;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Commands.CreatePostCategory
{
    public class CreatePostCategoryCommandValidator : AbstractValidator<CreatePostCategoryCommand>
    {
        public CreatePostCategoryCommandValidator(IValidator<CreatePostCategoryModel> createPostCategoryModelValidator)
        {
            RuleFor(e => e.Model).NotNull().SetValidator(createPostCategoryModelValidator);
        }
    }
}
