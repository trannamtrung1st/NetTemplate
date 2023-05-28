using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.Post.Models;
using NetTemplate.Blog.ApplicationCore.Post.Views;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Queries.GetPosts
{
    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, ListResponseModel<PostListItemModel>>
    {
        private readonly IValidator<GetPostsQuery> _validator;
        private readonly IPostViewManager _postViewManager;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPostsQueryHandler> _logger;

        public GetPostsQueryHandler(
            IValidator<GetPostsQuery> validator,
            IPostViewManager postViewManager,
            IPostRepository postRepository,
            IMapper mapper,
            ILogger<GetPostsQueryHandler> logger)
        {
            _validator = validator;
            _postViewManager = postViewManager;
            _postRepository = postRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ListResponseModel<PostListItemModel>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            return _postViewManager.IsPostAvailable
                ? await HandleUsingView(request, cancellationToken)
                : await HandleUsingRepository(request, cancellationToken);
        }

        private async Task<ListResponseModel<PostListItemModel>> HandleUsingView(GetPostsQuery request, CancellationToken cancellationToken)
        {
            PostListRequestModel model = request.Model;

            IQueryable<PostView> query = (await _postViewManager.GetPostViews()).AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(model.Terms))
            {
                query = query.Where(e => e.Title.Contains(model.Terms));
            }

            if (model.CategoryId != null)
            {
                query = query.Where(e => e.CategoryId == model.CategoryId);
            }

            // Counting
            int total = query.Count();

            // Sorting
            query = query.SortBy<PostView, PostListRequestModel, Enums.PostSortBy>(model,
                IsUseColumn: (sort) => sort switch
                {
                    Enums.PostSortBy.CategoryName => false,
                    _ => true,
                });

            // Paging
            query = query.Paging(model);

            // Projecting
            PostListItemModel[] list = _mapper.ProjectTo<PostListItemModel>(query).ToArray();

            return new ListResponseModel<PostListItemModel>(total, list);
        }

        // [NOTE] Optional, we can use views only
        private async Task<ListResponseModel<PostListItemModel>> HandleUsingRepository(GetPostsQuery request, CancellationToken cancellationToken)
        {
            PostListRequestModel model = request.Model;

            IQueryable<PostEntity> query = _postRepository.GetQuery();

            // Filtering
            if (!string.IsNullOrEmpty(model.Terms))
            {
                query = query.Where(e => e.Title.Contains(model.Terms));
            }

            if (model.CategoryId != null)
            {
                query = query.Where(e => e.Category.Id == model.CategoryId);
            }

            // Counting
            int total = query.Count();

            // Sorting
            query = query.SortBy<PostEntity, PostListRequestModel, Enums.PostSortBy>(model);

            // Paging
            query = query.Paging(model);

            // Projecting
            PostListItemModel[] list = await _mapper
                .ProjectTo<PostListItemModel>(query)
                .ToArrayAsync(cancellationToken);

            return new ListResponseModel<PostListItemModel>(total, list);
        }
    }
}
