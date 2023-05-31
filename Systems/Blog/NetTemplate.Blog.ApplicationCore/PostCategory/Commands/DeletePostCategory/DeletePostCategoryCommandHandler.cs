using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Commands.DeletePostCategory
{
    public class DeletePostCategoryCommandHandler : IRequestHandler<DeletePostCategoryCommand>
    {
        private readonly IValidator<DeletePostCategoryCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostCategoryRepository _postCategoryRepository;
        private readonly ILogger<DeletePostCategoryCommandHandler> _logger;

        public DeletePostCategoryCommandHandler(
            IValidator<DeletePostCategoryCommand> validator,
            IUnitOfWork unitOfWork,
            IPostCategoryRepository postCategoryRepository,
            ILogger<DeletePostCategoryCommandHandler> logger)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _postCategoryRepository = postCategoryRepository;
            _logger = logger;
        }

        public async Task Handle(DeletePostCategoryCommand request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            PostCategoryEntity entity = (await _postCategoryRepository.QueryById())
                .Select(e => new PostCategoryEntity(e.Id))
                .FirstOrDefault();

            if (entity == null) throw new NotFoundException();

            await _postCategoryRepository.Track(entity);

            entity.SoftDelete();

            await _unitOfWork.CommitChanges();
        }
    }
}
