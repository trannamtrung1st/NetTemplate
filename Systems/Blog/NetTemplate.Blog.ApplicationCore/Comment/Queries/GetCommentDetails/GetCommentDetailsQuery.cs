using MediatR;
using NetTemplate.Blog.ApplicationCore.Comment.Models;

namespace NetTemplate.Blog.ApplicationCore.Comment.Queries.GetCommentDetails
{
    public class GetCommentDetailsQuery : IRequest<CommentDetailsModel>
    {
        public int Id { get; }

        public GetCommentDetailsQuery(int id)
        {
            Id = id;
        }
    }
}
