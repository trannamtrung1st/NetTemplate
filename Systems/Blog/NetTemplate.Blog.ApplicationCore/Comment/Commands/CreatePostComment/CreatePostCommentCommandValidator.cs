using FluentValidation;
using NetTemplate.Blog.ApplicationCore.Comment.Models;

namespace NetTemplate.Blog.ApplicationCore.Comment.Commands.CreatePostComment
{
    public class CreatePostCommentCommandValidator : AbstractValidator<CreatePostCommentCommand>
    {
        public CreatePostCommentCommandValidator(IValidator<CreateCommentModel> createCommentModelValidator)
        {
            RuleFor(e => e.OnPostId).GreaterThan(0);

            RuleFor(e => e.Model).NotNull().SetValidator(createCommentModelValidator);
        }
    }
}
