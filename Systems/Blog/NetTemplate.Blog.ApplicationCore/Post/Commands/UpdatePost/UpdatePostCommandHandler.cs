using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.Post.Interfaces;
using NetTemplate.Blog.ApplicationCore.Post.Models;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.Post.Commands.UpdatePost
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand>
    {
        private readonly IValidator<UpdatePostCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostRepository _postRepository;
        private readonly IPostValidator _postValidator;
        private readonly ILogger<UpdatePostCommandHandler> _logger;

        public UpdatePostCommandHandler(
            IValidator<UpdatePostCommand> validator,
            IUnitOfWork unitOfWork,
            IPostRepository postRepository,
            IPostValidator postValidator,
            ILogger<UpdatePostCommandHandler> logger)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _postRepository = postRepository;
            _postValidator = postValidator;
            _logger = logger;
        }

        public async Task Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            UpdatePostModel model = request.Model;

            PostEntity entity = await _postRepository.FindById(request.Id);

            if (entity == null) throw new NotFoundException();

            await Validate(entity, model, cancellationToken);

            entity.UpdatePost(model.Title, model.Content, model.CategoryId);

            entity.UpdateTags(model.Tags);

            await _unitOfWork.CommitChanges(cancellationToken: cancellationToken);
        }

        private async Task Validate(PostEntity entity, UpdatePostModel model, CancellationToken cancellationToken)
        {
            await _postValidator.ValidateExistences(model.CategoryId, cancellationToken);

            await _postValidator.ValidatePostTitle(currentTitle: entity.Title, newTitle: model.Title, cancellationToken);
        }
    }
}
