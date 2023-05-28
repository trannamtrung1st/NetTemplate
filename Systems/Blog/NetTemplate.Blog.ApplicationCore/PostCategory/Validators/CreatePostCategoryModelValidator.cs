using FluentValidation;
using NetTemplate.Blog.ApplicationCore.PostCategory.Models;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Validators
{
    public class CreatePostCategoryModelValidator : AbstractValidator<CreatePostCategoryModel>
    {
        public CreatePostCategoryModelValidator(IValidator<BasePostCategoryModel> baseValidator)
        {
            Include(baseValidator);
        }
    }
}
