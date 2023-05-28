using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.PostCategory.Models;
using NetTemplate.Blog.ApplicationCore.PostCategory.Views;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Queries.GetPostCategoryDetails
{
    public class GetPostCategoryDetailsQueryHandler : IRequestHandler<GetPostCategoryDetailsQuery, PostCategoryDetailsModel>
    {
        private readonly IValidator<GetPostCategoryDetailsQuery> _validator;
        private readonly IPostCategoryViewManager _postCategoryViewManager;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPostCategoryDetailsQueryHandler> _logger;

        public GetPostCategoryDetailsQueryHandler(
            IValidator<GetPostCategoryDetailsQuery> validator,
            IPostCategoryViewManager postCategoryViewManager,
            IMapper mapper,
            ILogger<GetPostCategoryDetailsQueryHandler> logger)
        {
            _validator = validator;
            _postCategoryViewManager = postCategoryViewManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PostCategoryDetailsModel> Handle(GetPostCategoryDetailsQuery request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            PostCategoryView view = await _postCategoryViewManager.GetPostCategoryView(request.Id);

            if (view == null) throw new NotFoundException();

            PostCategoryDetailsModel model = _mapper.Map<PostCategoryDetailsModel>(view);

            return model;
        }
    }
}
