using MediatR;
using NetTemplate.Blog.ApplicationCore.PostCategory.Models;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Queries.GetPostCategoryDetails
{
    public class GetPostCategoryDetailsQuery : IRequest<PostCategoryDetailsModel>
    {
        public int Id { get; }

        public GetPostCategoryDetailsQuery(int id)
        {
            Id = id;
        }
    }
}
