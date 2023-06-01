using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.Post.Commands.UpdatePostTags
{
    public class UpdatePostTagsCommandHandler : IRequestHandler<UpdatePostTagsCommand>
    {
        private readonly IValidator<UpdatePostTagsCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostRepository _postRepository;
        private readonly ILogger<UpdatePostTagsCommandHandler> _logger;

        public UpdatePostTagsCommandHandler(
            IValidator<UpdatePostTagsCommand> validator,
            IUnitOfWork unitOfWork,
            IPostRepository postRepository,
            ILogger<UpdatePostTagsCommandHandler> logger)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task Handle(UpdatePostTagsCommand request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            Models.UpdatePostTagsModel model = request.Model;

            PostEntity entity = await _postRepository.FindById(request.Id);

            if (entity == null) throw new NotFoundException();

            entity.UpdateTags(model.Tags);

            await _unitOfWork.CommitChanges(cancellationToken: cancellationToken);
        }
    }
}
