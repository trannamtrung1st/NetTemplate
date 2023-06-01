using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Commands.UpdatePostCategory
{
    public class UpdatePostCategoryCommandHandler : IRequestHandler<UpdatePostCategoryCommand>
    {
        private readonly IValidator<UpdatePostCategoryCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostCategoryRepository _postCategoryRepository;
        private readonly ILogger<UpdatePostCategoryCommandHandler> _logger;

        public UpdatePostCategoryCommandHandler(
            IValidator<UpdatePostCategoryCommand> validator,
            IUnitOfWork unitOfWork,
            IPostCategoryRepository postCategoryRepository,
            ILogger<UpdatePostCategoryCommandHandler> logger)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _postCategoryRepository = postCategoryRepository;
            _logger = logger;
        }

        public async Task Handle(UpdatePostCategoryCommand request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            Models.UpdatePostCategoryModel model = request.Model;

            PostCategoryEntity entity = await _postCategoryRepository.FindById(request.Id);

            if (entity == null) throw new NotFoundException();

            entity.Update(model.Name);

            await _unitOfWork.CommitChanges(cancellationToken: cancellationToken);
        }
    }
}
