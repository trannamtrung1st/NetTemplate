using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.Post.Models;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.Post.Commands.CreatePost
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand>
    {
        private readonly IValidator<CreatePostCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostRepository _postRepository;
        private readonly ILogger<CreatePostCommandHandler> _logger;

        public CreatePostCommandHandler(
            IValidator<CreatePostCommand> validator,
            IUnitOfWork unitOfWork,
            IPostRepository postRepository,
            ILogger<CreatePostCommandHandler> logger)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            CreatePostModel model = request.Model;

            PostTagEntity[] tags = model.Tags?.Select(value => new PostTagEntity(value)).ToArray();

            PostEntity entity = new PostEntity(model.Title, model.Content, model.CategoryId, tags);

            await _postRepository.Create(entity);

            await _unitOfWork.CommitChanges();
        }
    }
}
