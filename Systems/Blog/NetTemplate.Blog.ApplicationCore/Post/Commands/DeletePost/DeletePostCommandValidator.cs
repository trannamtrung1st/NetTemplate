using FluentValidation;

namespace NetTemplate.Blog.ApplicationCore.Post.Commands.DeletePost
{
    public class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
    {
        public DeletePostCommandValidator()
        {
            RuleFor(e => e.PostId).GreaterThan(0);
        }
    }
}
