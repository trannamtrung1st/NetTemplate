using FluentValidation;
using NetTemplate.Blog.ApplicationCore.Post.Models;
using CommonConstraints = NetTemplate.Shared.ApplicationCore.Common.Constants.Constraints;

namespace NetTemplate.Blog.ApplicationCore.Post.Validators
{
    public class BasePostModelValidator : AbstractValidator<BasePostModel>
    {
        public BasePostModelValidator()
        {
            RuleFor(e => e.CategoryId).GreaterThan(0);

            RuleFor(e => e.Title).NotEmpty().MaximumLength(CommonConstraints.MaxStringLength);

            RuleFor(e => e.Content).NotEmpty();
        }
    }
}
