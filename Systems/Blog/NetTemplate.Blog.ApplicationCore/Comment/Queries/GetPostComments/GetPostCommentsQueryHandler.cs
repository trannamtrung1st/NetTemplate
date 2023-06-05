using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.Comment.Models;
using NetTemplate.Blog.ApplicationCore.Post;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.Comment.Queries.GetPostComments
{
    public class GetPostCommentsQueryHandler : IRequestHandler<GetPostCommentsQuery, ListResponseModel<CommentListItemModel>>
    {
        private readonly IValidator<GetPostCommentsQuery> _validator;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger<GetPostCommentsQueryHandler> _logger;

        public GetPostCommentsQueryHandler(
            IValidator<GetPostCommentsQuery> validator,
            ICommentRepository commentRepository,
            IPostRepository postRepository,
            ILogger<GetPostCommentsQueryHandler> logger)
        {
            _validator = validator;
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task<ListResponseModel<CommentListItemModel>> Handle(GetPostCommentsQuery request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            CommentListRequestModel model = request.Model;

            bool exists = await _postRepository.Exists(request.OnPostId, cancellationToken);

            if (!exists) throw new NotFoundException();

            QueryResponseModel<CommentListItemModel> response = await _commentRepository.Query<CommentListItemModel>(
                onPostId: request.OnPostId,
                creatorId: model.CreatorId,
                sortBy: new[] { Enums.CommentSortBy.CreatedTime },
                isDesc: new[] { false },
                paging: model,
                count: true,
                cancellationToken: cancellationToken);

            CommentListItemModel[] list = response.Query.ToArray();

            return new ListResponseModel<CommentListItemModel>(response.Total.Value, list);
        }
    }
}
