using FluentValidation;

namespace NetTemplate.Blog.ApplicationCore.Comment.Commands.DeleteComment
{
    public class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
    {
        public DeleteCommentCommandValidator()
        {
            RuleFor(e => e.Id).GreaterThan(0);
        }
    }
}
