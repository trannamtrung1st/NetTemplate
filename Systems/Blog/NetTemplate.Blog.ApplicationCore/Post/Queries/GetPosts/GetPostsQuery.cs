using MediatR;
using NetTemplate.Blog.ApplicationCore.Post.Models;
using NetTemplate.Shared.ApplicationCore.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Queries.GetPosts
{
    public class GetPostsQuery : IRequest<ListResponseModel<PostListItemModel>>
    {
        public PostListRequestModel Model { get; }

        public GetPostsQuery(PostListRequestModel model)
        {
            Model = model;
        }
    }
}
