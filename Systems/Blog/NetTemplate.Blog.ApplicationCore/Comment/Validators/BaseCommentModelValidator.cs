using FluentValidation;
using NetTemplate.Blog.ApplicationCore.Comment.Models;

namespace NetTemplate.Blog.ApplicationCore.Comment.Validators
{
    public class BaseCommentModelValidator : AbstractValidator<BaseCommentModel>
    {
        public BaseCommentModelValidator()
        {
            RuleFor(e => e.Content).NotEmpty().MaximumLength(CommentEntity.Constraints.MaxContentLength);
        }
    }
}
