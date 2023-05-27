using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.Post.Models;
using NetTemplate.Blog.ApplicationCore.Post.Views;
using NetTemplate.Common.Enumerations;
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

            return _postViewManager.IsAvailable
                ? await HandleUsingView(request, cancellationToken)
                : await HandleUsingRepository(request, cancellationToken);
        }

        private async Task<ListResponseModel<PostListItemModel>> HandleUsingView(GetPostsQuery request, CancellationToken cancellationToken)
        {
            PostListRequestModel model = request.Model;

            IQueryable<PostView> query = (await _postViewManager.GetPostViews()).AsQueryable();

            if (!string.IsNullOrEmpty(model.Terms))
            {
                query = query.Where(e => e.Title.Contains(model.Terms));
            }

            if (model.CategoryId != null)
            {
                query = query.Where(e => e.CategoryId == model.CategoryId);
            }

            int total = query.Count();

            // [TODO] refactor
            for (int i = 0; i < model.SortBy.Length; i++)
            {
                Enums.PostSortBy currentSort = model.SortBy[i];
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

            PostListItemModel[] list = _mapper.ProjectTo<PostListItemModel>(query).ToArray();

            return new ListResponseModel<PostListItemModel>(total, list);
        }

        // [NOTE] Optional, we can use views only
        private async Task<ListResponseModel<PostListItemModel>> HandleUsingRepository(GetPostsQuery request, CancellationToken cancellationToken)
        {
            PostListRequestModel model = request.Model;

            IQueryable<PostEntity> query = _postRepository.GetQuery();

            if (!string.IsNullOrEmpty(model.Terms))
            {
                query = query.Where(e => e.Title.Contains(model.Terms));
            }

            if (model.CategoryId != null)
            {
                query = query.Where(e => e.Category.Id == model.CategoryId);
            }

            int total = query.Count();

            // [TODO] refactor
            for (int i = 0; i < model.SortBy.Length; i++)
            {
                Enums.PostSortBy currentSort = model.SortBy[i];
                bool currentIsDesc = model.IsDesc[i];

                switch (currentSort)
                {
                    case Enums.PostSortBy.CategoryName:
                        {
                            query = query.SortSequential(e => e.Category.Name, currentIsDesc);
                            break;
                        }
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

            PostListItemModel[] list = await _mapper
                .ProjectTo<PostListItemModel>(query)
                .ToArrayAsync(cancellationToken);

            return new ListResponseModel<PostListItemModel>(total, list);
        }
    }
}
