using FluentValidation;
using NetTemplate.Blog.ApplicationCore.PostCategory.Models;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Validators
{
    public class UpdatePostCategoryModelValidator : AbstractValidator<UpdatePostCategoryModel>
    {
        public UpdatePostCategoryModelValidator(IValidator<BasePostCategoryModel> baseValidator)
        {
            Include(baseValidator);
        }
    }
}
