using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.PostCategory.Interfaces;
using NetTemplate.Blog.ApplicationCore.PostCategory.Models;
using NetTemplate.Blog.ApplicationCore.PostCategory.Views;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Queries.GetPostCategories
{
    public class GetPostCategoriesQueryHandler : IRequestHandler<GetPostCategoriesQuery, ListResponseModel<PostCategoryListItemModel>>
    {
        private readonly IValidator<GetPostCategoriesQuery> _validator;
        private readonly IPostCategoryViewManager _postCategoryViewManager;
        private readonly IPostCategoryRepository _postCategoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPostCategoriesQueryHandler> _logger;

        public GetPostCategoriesQueryHandler(
            IValidator<GetPostCategoriesQuery> validator,
            IPostCategoryViewManager postCategoryViewManager,
            IPostCategoryRepository postCategoryRepository,
            IMapper mapper,
            ILogger<GetPostCategoriesQueryHandler> logger)
        {
            _validator = validator;
            _postCategoryViewManager = postCategoryViewManager;
            _postCategoryRepository = postCategoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ListResponseModel<PostCategoryListItemModel>> Handle(GetPostCategoriesQuery request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            return _postCategoryViewManager.IsPostCategoryAvailable
                ? await HandleUsingView(request, cancellationToken)
                : await HandleUsingRepository(request, cancellationToken);
        }

        private async Task<ListResponseModel<PostCategoryListItemModel>> HandleUsingView(GetPostCategoriesQuery request, CancellationToken cancellationToken)
        {
            PostCategoryListRequestModel model = request.Model;

            ListResponseModel<PostCategoryView> response = await _postCategoryViewManager.FilterPostCategoryViews(
                terms: model.Terms,
                ids: model.Ids,
                sortBy: model.SortBy,
                isDesc: model.IsDesc,
                paging: model);

            PostCategoryListItemModel[] list = _mapper
                .ProjectTo<PostCategoryListItemModel>(response.List.AsQueryable())
                .ToArray();

            return new ListResponseModel<PostCategoryListItemModel>(response.Total, list);
        }

        // [OPTIONAL] we can use views only
        private async Task<ListResponseModel<PostCategoryListItemModel>> HandleUsingRepository(GetPostCategoriesQuery request, CancellationToken cancellationToken)
        {
            PostCategoryListRequestModel model = request.Model;

            QueryResponseModel<PostCategoryEntity> response = await _postCategoryRepository.Query(
                terms: model.Terms,
                ids: model.Ids,
                sortBy: model.SortBy,
                isDesc: model.IsDesc,
                paging: model,
                count: true);

            PostCategoryListItemModel[] list = _mapper
                .ProjectTo<PostCategoryListItemModel>(response.Query)
                .ToArray();

            return new ListResponseModel<PostCategoryListItemModel>(response.Total.Value, list);
        }
    }
}
