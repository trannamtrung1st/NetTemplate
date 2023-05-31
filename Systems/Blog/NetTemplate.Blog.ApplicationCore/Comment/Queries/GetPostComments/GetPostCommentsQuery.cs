using MediatR;
using NetTemplate.Blog.ApplicationCore.Comment.Models;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.Comment.Queries.GetPostComments
{
    public class GetPostCommentsQuery : IRequest<ListResponseModel<CommentListItemModel>>
    {
        public int OnPostId { get; set; }
        public CommentListRequestModel Model { get; }

        public GetPostCommentsQuery(int onPostId, CommentListRequestModel model)
        {
            OnPostId = onPostId;
            Model = model;
        }
    }
}
