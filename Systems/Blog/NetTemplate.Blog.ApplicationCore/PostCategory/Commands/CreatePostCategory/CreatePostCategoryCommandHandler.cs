using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.PostCategory.Interfaces;
using NetTemplate.Blog.ApplicationCore.PostCategory.Models;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Commands.CreatePostCategory
{
    public class CreatePostCategoryCommandHandler : IRequestHandler<CreatePostCategoryCommand>
    {
        private readonly IValidator<CreatePostCategoryCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostCategoryRepository _postCategoryRepository;
        private readonly IPostCategoryValidator _postCategoryValidator;
        private readonly ILogger<CreatePostCategoryCommandHandler> _logger;

        public CreatePostCategoryCommandHandler(
            IValidator<CreatePostCategoryCommand> validator,
            IUnitOfWork unitOfWork,
            IPostCategoryRepository postCategoryRepository,
            IPostCategoryValidator postCategoryValidator,
            ILogger<CreatePostCategoryCommandHandler> logger)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _postCategoryRepository = postCategoryRepository;
            _postCategoryValidator = postCategoryValidator;
            _logger = logger;
        }

        public async Task Handle(CreatePostCategoryCommand request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            CreatePostCategoryModel model = request.Model;

            await Validate(model, cancellationToken);

            PostCategoryEntity entity = new PostCategoryEntity(model.Name);

            await _postCategoryRepository.Create(entity, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken: cancellationToken);
        }

        private async Task Validate(CreatePostCategoryModel model, CancellationToken cancellationToken)
        {
            await _postCategoryValidator.ValidatePostCategoryName(currentName: null, newName: model.Name, cancellationToken);
        }
    }
}
