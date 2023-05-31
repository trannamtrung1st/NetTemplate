using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Models;
using NetTemplate.Shared.ApplicationCore.Domains.Identity.Interfaces;
using NetTemplate.Shared.ApplicationCore.Domains.Identity.Models;

namespace NetTemplate.Blog.ApplicationCore.User.Commands.SyncUsers
{
    public class SyncUsersCommandHandler : IRequestHandler<SyncUsersCommand>
    {
        private readonly IValidator<SyncUsersCommand> _validator;
        private readonly IIdentityService _identityService;
        private readonly IUserPartialRepository _userPartialRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SyncUsersCommandHandler> _logger;

        public SyncUsersCommandHandler(
            IValidator<SyncUsersCommand> validator,
            IIdentityService identityService,
            IUserPartialRepository userPartialRepository,
            IUnitOfWork unitOfWork,
            ILogger<SyncUsersCommandHandler> logger)
        {
            _validator = validator;
            _identityService = identityService;
            _userPartialRepository = userPartialRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(SyncUsersCommand request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            IEnumerable<IdentityUserModel> identityUsers = await _identityService.GetIdentityUsers(cancellationToken);

            string[] userCodes = identityUsers.Select(o => o.UserCode).ToArray();

            QueryResponseModel<UserPartialEntity> response = await _userPartialRepository.Query<UserPartialEntity>(
                userCodes: userCodes);

            Dictionary<string, UserPartialEntity> userMap = response.Query.ToDictionary(o => o.UserCode);

            foreach (var identityUser in identityUsers)
            {
                try
                {
                    if (userMap.TryGetValue(identityUser.UserCode, out UserPartialEntity currentUser))
                    {
                        currentUser = new UserPartialEntity(
                            identityUser.UserCode,
                            identityUser.FirstName,
                            identityUser.LastName,
                            identityUser.IsActive);

                        await _userPartialRepository.Create(currentUser);
                    }
                    else
                    {
                        currentUser.UpdateInfo(identityUser.FirstName, identityUser.LastName);
                        currentUser.SetActive(identityUser.IsActive);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[ERROR] Failed to sync user {UserCode}", identityUser.UserCode);
                }
            }

            await _unitOfWork.CommitChanges();
        }
    }
}
