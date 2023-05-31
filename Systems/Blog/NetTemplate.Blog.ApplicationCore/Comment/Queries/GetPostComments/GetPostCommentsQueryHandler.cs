using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.Comment.Models;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.Comment.Queries.GetPostComments
{
    public class GetPostCommentsQueryHandler : IRequestHandler<GetPostCommentsQuery, ListResponseModel<CommentListItemModel>>
    {
        private readonly IValidator<GetPostCommentsQuery> _validator;
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger<GetPostCommentsQueryHandler> _logger;

        public GetPostCommentsQueryHandler(
            IValidator<GetPostCommentsQuery> validator,
            ICommentRepository commentRepository,
            ILogger<GetPostCommentsQueryHandler> logger)
        {
            _validator = validator;
            _commentRepository = commentRepository;
            _logger = logger;
        }

        public async Task<ListResponseModel<CommentListItemModel>> Handle(GetPostCommentsQuery request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            CommentListRequestModel model = request.Model;

            QueryResponseModel<CommentListItemModel> response = await _commentRepository.Query<CommentListItemModel>(
                onPostId: request.OnPostId,
                creatorId: model.CreatorId,
                sortBy: new[] { Enums.CommentSortBy.CreatedTime },
                isDesc: new[] { false },
                paging: model,
                count: true);

            CommentListItemModel[] list = response.Query.ToArray();

            return new ListResponseModel<CommentListItemModel>(response.Total.Value, list);
        }
    }
}
