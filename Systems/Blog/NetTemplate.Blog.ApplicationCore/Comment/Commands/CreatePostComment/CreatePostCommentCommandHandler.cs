using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.Comment.Interfaces;
using NetTemplate.Blog.ApplicationCore.Comment.Models;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.Comment.Commands.CreatePostComment
{
    public class CreatePostCommentCommandHandler : IRequestHandler<CreatePostCommentCommand>
    {
        private readonly IValidator<CreatePostCommentCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommentRepository _commentRepository;
        private readonly ICommentValidator _commentValidator;
        private readonly ILogger<CreatePostCommentCommandHandler> _logger;

        public CreatePostCommentCommandHandler(
            IValidator<CreatePostCommentCommand> validator,
            IUnitOfWork unitOfWork,
            ICommentRepository commentRepository,
            ICommentValidator commentValidator,
            ILogger<CreatePostCommentCommandHandler> logger)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _commentRepository = commentRepository;
            _commentValidator = commentValidator;
            _logger = logger;
        }

        public async Task Handle(CreatePostCommentCommand request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            CreateCommentModel model = request.Model;

            await Validate(request.OnPostId, model, cancellationToken);

            CommentEntity entity = new CommentEntity(model.Content, request.OnPostId);

            await _commentRepository.Create(entity, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken: cancellationToken);
        }

        private async Task Validate(int postId, CreateCommentModel model, CancellationToken cancellationToken)
        {
            await _commentValidator.ValidateExistences(postId, cancellationToken);
        }
    }
}
