using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Shared.ApplicationCore.Exceptions;
using NetTemplate.Shared.ApplicationCore.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.Post.Commands.UpdatePost
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand>
    {
        private readonly IValidator<UpdatePostCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostRepository _postRepository;
        private readonly ILogger<UpdatePostCommandHandler> _logger;

        public UpdatePostCommandHandler(
            IValidator<UpdatePostCommand> validator,
            IUnitOfWork unitOfWork,
            IPostRepository postRepository,
            ILogger<UpdatePostCommandHandler> logger)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            Models.UpdatePostModel model = request.Model;

            PostEntity entity = await _postRepository.FindByIdAsync(model.Id);

            if (entity == null) throw new NotFoundException();

            entity.UpdatePost(model.Title, model.Content, model.CategoryId);

            entity.UpdateTags(model.Tags);

            await _unitOfWork.CommitChanges();
        }
    }
}
