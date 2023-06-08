using FluentValidation;

namespace NetTemplate.Blog.ApplicationCore.User.Commands.SyncNewUser
{
    public class SyncNewUserCommandValidator : AbstractValidator<SyncNewUserCommand>
    {
        public SyncNewUserCommandValidator()
        {
            RuleFor(e => e.Model).NotNull();
        }
    }
}
