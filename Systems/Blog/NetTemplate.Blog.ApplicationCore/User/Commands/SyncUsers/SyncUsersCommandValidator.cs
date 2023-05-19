using FluentValidation;

namespace NetTemplate.Blog.ApplicationCore.User.Commands.SyncUsers
{
    public class SyncUsersCommandValidator : AbstractValidator<SyncUsersCommand>
    {
        public SyncUsersCommandValidator()
        {
        }
    }
}
