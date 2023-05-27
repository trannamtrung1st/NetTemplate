using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.PostCategory.Models;
using NetTemplate.Blog.ApplicationCore.PostCategory.Views;
using NetTemplate.Common.Enumerations;
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

            return _postCategoryViewManager.IsAvailable
                ? await HandleUsingView(request, cancellationToken)
                : await HandleUsingRepository(request, cancellationToken);
        }

        private async Task<ListResponseModel<PostCategoryListItemModel>> HandleUsingView(GetPostCategoriesQuery request, CancellationToken cancellationToken)
        {
            PostCategoryListRequestModel model = request.Model;

            IQueryable<PostCategoryView> query = (await _postCategoryViewManager.GetPostCategoryViews()).AsQueryable();

            if (!string.IsNullOrEmpty(model.Terms))
            {
                query = query.Where(e => e.Name.Contains(model.Terms));
            }

            if (model.Ids?.Any() == true)
            {
                query = query.ByIds(model.Ids);
            }

            int total = query.Count();

            // [TODO] refactor
            for (int i = 0; i < model.SortBy.Length; i++)
            {
                Enums.PostCategorySortBy currentSort = model.SortBy[i];
                bool currentIsDesc = model.IsDesc[i];

                switch (currentSort)
                {
                    default:
                        {
                            string columnName = currentSort.GetName();
                            query = query.SortSequential(columnName, currentIsDesc);
                            break;
                        }
                }
            }

            query = query.Skip(model.Skip);

            if (!model.CanGetAll() || model.Take != null)
            {
                query = query.Take(model.GetTakeOrDefault());
            }

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

            if (!string.IsNullOrEmpty(model.Terms))
            {
                query = query.Where(e => e.Name.Contains(model.Terms));
            }

            if (model.Ids?.Any() == true)
            {
                query = query.ByIds(model.Ids);
            }

            int total = query.Count();

            // [TODO] refactor
            for (int i = 0; i < model.SortBy.Length; i++)
            {
                Enums.PostCategorySortBy currentSort = model.SortBy[i];
                bool currentIsDesc = model.IsDesc[i];

                switch (currentSort)
                {
                    default:
                        {
                            string columnName = currentSort.GetName();
                            query = query.SortSequential(columnName, currentIsDesc);
                            break;
                        }
                }
            }

            query = query.Skip(model.Skip);

            if (!model.CanGetAll() || model.Take != null)
            {
                query = query.Take(model.GetTakeOrDefault());
            }

            PostCategoryListItemModel[] list = await _mapper
                .ProjectTo<PostCategoryListItemModel>(query)
                .ToArrayAsync(cancellationToken);

            return new ListResponseModel<PostCategoryListItemModel>(total, list);
        }
    }
}
