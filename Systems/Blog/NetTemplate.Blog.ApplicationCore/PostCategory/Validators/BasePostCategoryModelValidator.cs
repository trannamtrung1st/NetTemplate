using FluentValidation;
using NetTemplate.Blog.ApplicationCore.PostCategory.Models;
using NetTemplate.Shared.ApplicationCore.Common.Entities;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Validators
{
    public class BasePostCategoryModelValidator : AbstractValidator<BasePostCategoryModel>
    {
        public BasePostCategoryModelValidator()
        {
            RuleFor(e => e.Name).NotEmpty().MaximumLength(AppEntity.Constraints.MaxStringLength);
        }
    }
}
