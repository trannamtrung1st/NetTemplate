using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.PostCategory.Interfaces;
using NetTemplate.Blog.ApplicationCore.PostCategory.Models;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Commands.UpdatePostCategory
{
    public class UpdatePostCategoryCommandHandler : IRequestHandler<UpdatePostCategoryCommand>
    {
        private readonly IValidator<UpdatePostCategoryCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostCategoryRepository _postCategoryRepository;
        private readonly IPostCategoryValidator _postCategoryValidator;
        private readonly ILogger<UpdatePostCategoryCommandHandler> _logger;

        public UpdatePostCategoryCommandHandler(
            IValidator<UpdatePostCategoryCommand> validator,
            IUnitOfWork unitOfWork,
            IPostCategoryRepository postCategoryRepository,
            IPostCategoryValidator postCategoryValidator,
            ILogger<UpdatePostCategoryCommandHandler> logger)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _postCategoryRepository = postCategoryRepository;
            _postCategoryValidator = postCategoryValidator;
            _logger = logger;
        }

        public async Task Handle(UpdatePostCategoryCommand request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            UpdatePostCategoryModel model = request.Model;

            PostCategoryEntity entity = await _postCategoryRepository.FindById(request.Id);

            if (entity == null) throw new NotFoundException();

            await Validate(model, cancellationToken);

            entity.Update(model.Name);

            await _unitOfWork.CommitChanges(cancellationToken: cancellationToken);
        }

        private async Task Validate(UpdatePostCategoryModel model, CancellationToken cancellationToken)
        {
            await _postCategoryValidator.ValidatePostCategoryName(model.Name, cancellationToken);
        }
    }
}
