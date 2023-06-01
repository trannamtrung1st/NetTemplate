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
        private readonly ILogger<GetPostsQueryHandler> _logger;

        public GetPostsQueryHandler(
            IValidator<GetPostsQuery> validator,
            IPostRepository postRepository,
            ILogger<GetPostsQueryHandler> logger)
        {
            _validator = validator;
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task<ListResponseModel<PostListItemModel>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            PostListRequestModel model = request.Model;

            QueryResponseModel<PostListItemModel> response = await _postRepository.Query<PostListItemModel>(
                terms: model.Terms,
                ids: model.Ids,
                categoryId: model.CategoryId,
                sortBy: model.SortBy,
                isDesc: model.IsDesc,
                paging: model,
                count: true,
                cancellationToken: cancellationToken);

            PostListItemModel[] list = response.Query.ToArray();

            return new ListResponseModel<PostListItemModel>(response.Total.Value, list);
        }
    }
}
