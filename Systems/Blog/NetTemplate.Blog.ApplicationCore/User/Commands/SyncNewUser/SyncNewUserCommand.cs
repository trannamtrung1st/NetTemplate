using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using NetTemplate.Shared.ApplicationCore.Domains.Identity.Models;

namespace NetTemplate.Blog.ApplicationCore.User.Commands.SyncNewUser
{
    public class SyncNewUserCommand : ITransactionalCommand
    {
        public IdentityUserModel Model { get; }

        public SyncNewUserCommand(IdentityUserModel model)
        {
            Model = model;
        }
    }
}
