using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.Post.Models;
using NetTemplate.Blog.ApplicationCore.Post.Views;
using NetTemplate.Shared.ApplicationCore.Models;

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

            return _postViewManager.IsReady
                ? await HandleUsingView(request, cancellationToken)
                : await HandleUsingRepository(request, cancellationToken);
        }

        private async Task<ListResponseModel<PostListItemModel>> HandleUsingView(GetPostsQuery request, CancellationToken cancellationToken)
        {
            PostListRequestModel model = request.Model;

            IQueryable<PostView> views = (await _postViewManager.GetPostViews()).AsQueryable();

            if (!string.IsNullOrEmpty(model.Terms))
            {
                views = views.Where(e => e.Title.Contains(model.Terms));
            }

            if (model.CategoryId != null)
            {
                views = views.Where(e => e.Category.Id == model.CategoryId);
            }

            int total = views.Count();

            views = views.Skip(model.Skip);

            if (!model.CanGetAll() || model.Take != null)
            {
                views = views.Take(model.GetTakeOrDefault());
            }

            PostListItemModel[] list = _mapper.ProjectTo<PostListItemModel>(views).ToArray();

            return new ListResponseModel<PostListItemModel>(total, list);
        }

        // [NOTE] Optional, we can use views only
        private async Task<ListResponseModel<PostListItemModel>> HandleUsingRepository(GetPostsQuery request, CancellationToken cancellationToken)
        {
            PostListRequestModel model = request.Model;

            IQueryable<PostEntity> views = _postRepository.GetQuery();

            if (!string.IsNullOrEmpty(model.Terms))
            {
                views = views.Where(e => e.Title.Contains(model.Terms));
            }

            if (model.CategoryId != null)
            {
                views = views.Where(e => e.Category.Id == model.CategoryId);
            }

            int total = views.Count();

            views = views.Skip(model.Skip);

            if (!model.CanGetAll() || model.Take != null)
            {
                views = views.Take(model.GetTakeOrDefault());
            }

            PostListItemModel[] list = await _mapper
                .ProjectTo<PostListItemModel>(views)
                .ToArrayAsync(cancellationToken);

            return new ListResponseModel<PostListItemModel>(total, list);
        }
    }
}
