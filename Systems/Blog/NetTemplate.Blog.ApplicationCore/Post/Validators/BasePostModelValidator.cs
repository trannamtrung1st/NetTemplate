using FluentValidation;
using NetTemplate.Blog.ApplicationCore.Post.Models;
using NetTemplate.Shared.ApplicationCore.Common.Entities;

namespace NetTemplate.Blog.ApplicationCore.Post.Validators
{
    public class BasePostModelValidator : AbstractValidator<BasePostModel>
    {
        public BasePostModelValidator()
        {
            RuleFor(e => e.CategoryId).GreaterThan(0);

            RuleFor(e => e.Title).NotEmpty().MaximumLength(AppEntity.Constraints.MaxStringLength);

            RuleFor(e => e.Content).NotEmpty();
        }
    }
}
