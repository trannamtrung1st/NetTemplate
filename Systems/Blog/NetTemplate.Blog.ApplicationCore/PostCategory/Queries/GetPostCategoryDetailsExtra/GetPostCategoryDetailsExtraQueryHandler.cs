using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly ILogger<GetPostCategoryDetailsExtraQueryHandler> _logger;

        public GetPostCategoryDetailsExtraQueryHandler(
            IValidator<GetPostCategoryDetailsExtraQuery> validator,
            IPostRepository postRepository,
            IMapper mapper,
            ILogger<GetPostCategoryDetailsExtraQueryHandler> logger)
        {
            _validator = validator;
            _postRepository = postRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PostCategoryDetailsExtraModel> Handle(GetPostCategoryDetailsExtraQuery request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            int postCount = await _postRepository.CountByCategory(request.Id);

            IQueryable<PostEntity> latestPostQuery = await _postRepository.GetLatestPostOfCategory(request.Id);

            LatestPostModel latestPost = _mapper.ProjectTo<LatestPostModel>(latestPostQuery).FirstOrDefault();

            PostCategoryDetailsExtraModel model = new PostCategoryDetailsExtraModel()
            {
                PostCount = postCount,
                LatestPost = latestPost
            };

            return model;
        }
    }
}
