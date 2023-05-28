using FluentValidation;
using NetTemplate.Blog.ApplicationCore.PostCategory.Models;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Commands.UpdatePostCategory
{
    public class UpdatePostCategoryCommandValidator : AbstractValidator<UpdatePostCategoryCommand>
    {
        public UpdatePostCategoryCommandValidator(IValidator<UpdatePostCategoryModel> updatePostCategoryModelValidator)
        {
            RuleFor(e => e.Id).GreaterThan(0);

            RuleFor(e => e.Model).NotNull().SetValidator(updatePostCategoryModelValidator);
        }
    }
}
