using MediatR;
using NetTemplate.Blog.ApplicationCore.PostCategory.Models;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Queries.GetPostCategories
{
    public class GetPostCategoriesQuery : IRequest<ListResponseModel<PostCategoryListItemModel>>
    {
        public PostCategoryListRequestModel Model { get; }

        public GetPostCategoriesQuery(PostCategoryListRequestModel model)
        {
            Model = model;
        }
    }
}
