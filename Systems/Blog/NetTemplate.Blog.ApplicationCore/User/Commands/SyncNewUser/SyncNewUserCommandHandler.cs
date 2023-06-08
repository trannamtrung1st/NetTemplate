using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using NetTemplate.Shared.ApplicationCore.Domains.Identity.Models;

namespace NetTemplate.Blog.ApplicationCore.User.Commands.SyncNewUser
{
    public class SyncNewUserCommandHandler : IRequestHandler<SyncNewUserCommand>
    {
        private readonly IValidator<SyncNewUserCommand> _validator;
        private readonly IUserPartialRepository _userPartialRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SyncNewUserCommandHandler> _logger;

        public SyncNewUserCommandHandler(
            IValidator<SyncNewUserCommand> validator,
            IUserPartialRepository userPartialRepository,
            IUnitOfWork unitOfWork,
            ILogger<SyncNewUserCommandHandler> logger)
        {
            _validator = validator;
            _userPartialRepository = userPartialRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(SyncNewUserCommand request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            IdentityUserModel model = request.Model;

            bool exists = await _userPartialRepository.Exists(model.UserCode);

            if (exists) throw new BusinessException(ResultCodes.User.UserAlreadyExists);

            UserPartialEntity entity = new UserPartialEntity(
                model.UserCode,
                model.FirstName,
                model.LastName,
                model.IsActive);

            await _userPartialRepository.Create(entity, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken: cancellationToken);
        }
    }
}
