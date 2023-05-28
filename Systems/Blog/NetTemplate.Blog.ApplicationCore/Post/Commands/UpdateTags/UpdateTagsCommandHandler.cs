using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.Post.Commands.UpdateTags
{
    public class UpdateTagsCommandHandler : IRequestHandler<UpdateTagsCommand>
    {
        private readonly IValidator<UpdateTagsCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostRepository _postRepository;
        private readonly ILogger<UpdateTagsCommandHandler> _logger;

        public UpdateTagsCommandHandler(
            IValidator<UpdateTagsCommand> validator,
            IUnitOfWork unitOfWork,
            IPostRepository postRepository,
            ILogger<UpdateTagsCommandHandler> logger)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task Handle(UpdateTagsCommand request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            Models.UpdatePostTagsModel model = request.Model;

            PostEntity entity = await _postRepository.FindById(request.Id);

            if (entity == null) throw new NotFoundException();

            entity.UpdateTags(model.Tags);

            await _unitOfWork.CommitChanges();
        }
    }
}
