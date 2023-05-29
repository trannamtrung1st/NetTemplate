using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

            IQueryable<PostCategoryView> query = (await _postCategoryViewManager.GetPostCategoryViews()).AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(model.Terms))
            {
                query = query.Where(e => e.Name.Contains(model.Terms, StringComparison.OrdinalIgnoreCase));
            }

            query = query.ByIdsIfAny(model.Ids);

            // Counting
            int total = query.Count();

            // Sorting
            query = query.SortBy(model.SortBy, model.IsDesc);

            // Paging
            query = query.Paging(model);

            // Projecting
            PostCategoryListItemModel[] list = _mapper
                .ProjectTo<PostCategoryListItemModel>(query)
                .ToArray();

            return new ListResponseModel<PostCategoryListItemModel>(total, list);
        }

        // [OPTIONAL] we can use views only
        private async Task<ListResponseModel<PostCategoryListItemModel>> HandleUsingRepository(GetPostCategoriesQuery request, CancellationToken cancellationToken)
        {
            PostCategoryListRequestModel model = request.Model;

            IQueryable<PostCategoryEntity> query = _postCategoryRepository.GetQuery();

            // Filtering
            if (!string.IsNullOrEmpty(model.Terms))
            {
                query = query.Where(e => e.Name.Contains(model.Terms));
            }

            query = query.ByIdsIfAny(model.Ids);

            // Counting
            int total = query.Count();

            // Sorting
            query = query.SortBy(model.SortBy, model.IsDesc,
                Process: (query, sort, isDesc) => sort switch
                {
                    Enums.PostCategorySortBy.CreatorFullName => query.SortSequential(PostCategoryEntity.CreatorFullNameExpression, isDesc),
                    _ => null,
                });

            // Paging
            query = query.Paging(model);

            // Projecting
            PostCategoryListItemModel[] list = await _mapper
                .ProjectTo<PostCategoryListItemModel>(query)
                .ToArrayAsync(cancellationToken);

            return new ListResponseModel<PostCategoryListItemModel>(total, list);
        }
    }
}
