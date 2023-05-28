using FluentValidation;
using NetTemplate.Blog.ApplicationCore.PostCategory.Models;
using CommonConstraints = NetTemplate.Shared.ApplicationCore.Common.Constants.Constraints;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Validators
{
    public class BasePostCategoryModelValidator : AbstractValidator<BasePostCategoryModel>
    {
        public BasePostCategoryModelValidator()
        {
            RuleFor(e => e.Name).NotEmpty().MaximumLength(CommonConstraints.MaxStringLength);
        }
    }
}
