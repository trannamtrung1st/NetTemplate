using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.Comment.Interfaces;
using NetTemplate.Blog.ApplicationCore.Comment.Models;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.Comment.Commands.UpdateComment
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand>
    {
        private readonly IValidator<UpdateCommentCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommentRepository _commentRepository;
        private readonly ICommentValidator _commentValidator;
        private readonly ILogger<UpdateCommentCommandHandler> _logger;

        public UpdateCommentCommandHandler(
            IValidator<UpdateCommentCommand> validator,
            IUnitOfWork unitOfWork,
            ICommentRepository commentRepository,
            ICommentValidator commentValidator,
            ILogger<UpdateCommentCommandHandler> logger)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _commentRepository = commentRepository;
            _commentValidator = commentValidator;
            _logger = logger;
        }

        public async Task Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            UpdateCommentModel model = request.Model;

            CommentEntity entity = await _commentRepository.FindById(request.Id);

            if (entity == null) throw new NotFoundException();

            await Validate(model, cancellationToken);

            entity.Update(model.Content);

            await _unitOfWork.CommitChanges(cancellationToken: cancellationToken);
        }

        private Task Validate(UpdateCommentModel model, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
