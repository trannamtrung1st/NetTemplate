using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.Post.Interfaces;
using NetTemplate.Blog.ApplicationCore.Post.Models;
using NetTemplate.Blog.ApplicationCore.Post.Views;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;

namespace NetTemplate.Blog.ApplicationCore.Post.Queries.GetPostDetails
{
    public class GetPostDetailsQueryHandler : IRequestHandler<GetPostDetailsQuery, PostDetailsModel>
    {
        private readonly IValidator<GetPostDetailsQuery> _validator;
        private readonly IPostRepository _postRepository;
        private readonly IPostViewManager _postViewManager;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPostDetailsQueryHandler> _logger;

        public GetPostDetailsQueryHandler(
            IValidator<GetPostDetailsQuery> validator,
            IPostRepository postRepository,
            IPostViewManager postViewManager,
            IMapper mapper,
            ILogger<GetPostDetailsQueryHandler> logger)
        {
            _validator = validator;
            _postRepository = postRepository;
            _postViewManager = postViewManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PostDetailsModel> Handle(GetPostDetailsQuery request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            PostView view = await _postViewManager.GetPostView(request.Id, cancellationToken);

            if (view == null) throw new NotFoundException();

            PostDetailsModel model = _mapper.Map<PostDetailsModel>(view);

            return model;
        }

        // [NOTE] sample
        public async Task<PostDetailsModel> HandleUsingRepository(GetPostDetailsQuery request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            IQueryable<PostDetailsModel> query = await _postRepository.QueryById<PostDetailsModel>(request.Id, cancellationToken);

            PostDetailsModel model = query.FirstOrDefault();

            if (model == null) throw new NotFoundException();

            return model;
        }
    }
}
