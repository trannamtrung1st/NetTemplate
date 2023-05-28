using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.Post.Models;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Queries.GetPosts
{
    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, ListResponseModel<PostListItemModel>>
    {
        private readonly IValidator<GetPostsQuery> _validator;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPostsQueryHandler> _logger;

        public GetPostsQueryHandler(
            IValidator<GetPostsQuery> validator,
            IPostRepository postRepository,
            IMapper mapper,
            ILogger<GetPostsQueryHandler> logger)
        {
            _validator = validator;
            _postRepository = postRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ListResponseModel<PostListItemModel>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

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
