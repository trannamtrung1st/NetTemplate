using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.Post.Interfaces;
using NetTemplate.Blog.ApplicationCore.Post.Models;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.Post.Commands.CreatePost
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand>
    {
        private readonly IValidator<CreatePostCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostRepository _postRepository;
        private readonly IPostValidator _postValidator;
        private readonly ILogger<CreatePostCommandHandler> _logger;

        public CreatePostCommandHandler(
            IValidator<CreatePostCommand> validator,
            IUnitOfWork unitOfWork,
            IPostRepository postRepository,
            IPostValidator postValidator,
            ILogger<CreatePostCommandHandler> logger)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _postRepository = postRepository;
            _postValidator = postValidator;
            _logger = logger;
        }

        public async Task Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            CreatePostModel model = request.Model;

            await Validate(model, cancellationToken);

            PostTagEntity[] tags = model.Tags?.Select(value => new PostTagEntity(value)).ToArray();

            PostEntity entity = new PostEntity(model.Title, model.Content, model.CategoryId, tags);

            await _postRepository.Create(entity, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken: cancellationToken);
        }

        private async Task Validate(CreatePostModel model, CancellationToken cancellationToken)
        {
            await _postValidator.ValidateExistences(model.CategoryId, cancellationToken);

            await _postValidator.ValidatePostTitle(currentTitle: null, newTitle: model.Title, cancellationToken);
        }
    }
}
