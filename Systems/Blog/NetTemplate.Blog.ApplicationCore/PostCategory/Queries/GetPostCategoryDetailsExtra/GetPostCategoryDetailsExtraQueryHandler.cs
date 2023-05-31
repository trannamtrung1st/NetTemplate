using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.Post;
using NetTemplate.Blog.ApplicationCore.PostCategory.Models;
using static NetTemplate.Blog.ApplicationCore.PostCategory.Models.PostCategoryDetailsExtraModel;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Queries.GetPostCategoryDetailsExtra
{
    public class GetPostCategoryDetailsExtraQueryHandler : IRequestHandler<GetPostCategoryDetailsExtraQuery, PostCategoryDetailsExtraModel>
    {
        private readonly IValidator<GetPostCategoryDetailsExtraQuery> _validator;
        private readonly IPostRepository _postRepository;
        private readonly ILogger<GetPostCategoryDetailsExtraQueryHandler> _logger;

        public GetPostCategoryDetailsExtraQueryHandler(
            IValidator<GetPostCategoryDetailsExtraQuery> validator,
            IPostRepository postRepository,
            ILogger<GetPostCategoryDetailsExtraQueryHandler> logger)
        {
            _validator = validator;
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task<PostCategoryDetailsExtraModel> Handle(GetPostCategoryDetailsExtraQuery request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            int postCount = await _postRepository.CountByCategory(request.Id);

            LatestPostModel latestPost = await _postRepository.GetLatestPostOfCategory<LatestPostModel>(request.Id);

            PostCategoryDetailsExtraModel model = new PostCategoryDetailsExtraModel()
            {
                PostCount = postCount,
                LatestPost = latestPost
            };

            return model;
        }
    }
}
