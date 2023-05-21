using FluentValidation;
using NetTemplate.Blog.ApplicationCore.Post.Models;
using SharedConstraints = NetTemplate.Shared.ApplicationCore.Common.Constants.Constraints;

namespace NetTemplate.Blog.ApplicationCore.Post.Validators
{
    public class BasePostModelValidator : AbstractValidator<BasePostModel>
    {
        public BasePostModelValidator()
        {
            RuleFor(e => e.CategoryId).GreaterThan(0);

            RuleFor(e => e.Title).NotEmpty().MaximumLength(SharedConstraints.Common.MaxStringLength);

            RuleFor(e => e.Content).NotEmpty();
        }
    }
}
