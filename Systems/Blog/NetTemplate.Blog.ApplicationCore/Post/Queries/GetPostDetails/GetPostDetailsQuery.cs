using MediatR;
using NetTemplate.Blog.ApplicationCore.Post.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Queries.GetPostDetails
{
    public class GetPostDetailsQuery : IRequest<PostDetailsModel>
    {
        public int Id { get; }

        public GetPostDetailsQuery(int id)
        {
            Id = id;
        }
    }
}
