using MediatR;
using NetTemplate.Blog.ApplicationCore.PostCategory.Models;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Queries.GetPostCategoryDetailsExtra
{
    public class GetPostCategoryDetailsExtraQuery : IRequest<PostCategoryDetailsExtraModel>
    {
        public int Id { get; }

        public GetPostCategoryDetailsExtraQuery(int id)
        {
            Id = id;
        }
    }
}
