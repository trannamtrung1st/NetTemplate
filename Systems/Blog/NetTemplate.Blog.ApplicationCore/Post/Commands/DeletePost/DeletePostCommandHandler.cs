using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.Post.Commands.DeletePost
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand>
    {
        private readonly IValidator<DeletePostCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostRepository _postRepository;
        private readonly ILogger<DeletePostCommandHandler> _logger;

        public DeletePostCommandHandler(
            IValidator<DeletePostCommand> validator,
            IUnitOfWork unitOfWork,
            IPostRepository postRepository,
            ILogger<DeletePostCommandHandler> logger)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            PostEntity entity = _postRepository.GetQuery()
                .Where(e => e.Id == request.PostId)
                .Select(e => new PostEntity(e.Id))
                .FirstOrDefault();

            if (entity == null) throw new NotFoundException();

            entity.SoftDelete();

            await _unitOfWork.CommitChanges();
        }
    }
}
