using AutoMapper;
using FluentValidation;
using MediatR;
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

            QueryResponseModel<PostEntity> response = await _postRepository.Query(
                terms: model.Terms,
                ids: model.Ids,
                categoryId: model.CategoryId,
                sortBy: model.SortBy,
                isDesc: model.IsDesc,
                paging: model,
                count: true);

            PostListItemModel[] list = _mapper.ProjectTo<PostListItemModel>(response.Query).ToArray();

            return new ListResponseModel<PostListItemModel>(response.Total.Value, list);
        }
    }
}
