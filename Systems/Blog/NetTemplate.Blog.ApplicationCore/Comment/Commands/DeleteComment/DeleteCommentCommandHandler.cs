using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.Comment.Commands.DeleteComment
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand>
    {
        private readonly IValidator<DeleteCommentCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger<DeleteCommentCommandHandler> _logger;

        public DeleteCommentCommandHandler(
            IValidator<DeleteCommentCommand> validator,
            IUnitOfWork unitOfWork,
            ICommentRepository commentRepository,
            ILogger<DeleteCommentCommandHandler> logger)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _commentRepository = commentRepository;
            _logger = logger;
        }

        public async Task Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            CommentEntity entity = (await _commentRepository.QueryById<CommentEntity>(request.Id, cancellationToken))
                .Select(e => new CommentEntity(e.Id))
                .FirstOrDefault();

            if (entity == null) throw new NotFoundException();

            await _commentRepository.Track(entity, cancellationToken);

            entity.SoftDelete();

            await _unitOfWork.CommitChanges(cancellationToken: cancellationToken);
        }
    }
}
