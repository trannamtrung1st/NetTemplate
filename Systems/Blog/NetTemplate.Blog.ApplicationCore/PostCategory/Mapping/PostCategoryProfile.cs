using AutoMapper;
using NetTemplate.Blog.ApplicationCore.PostCategory.Models;
using NetTemplate.Blog.ApplicationCore.PostCategory.Views;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Mapping
{
    public class PostCategoryProfile : Profile
    {
        public PostCategoryProfile()
        {
            CreateMap<PostCategoryEntity, PostCategoryView>();

            CreateMap<PostCategoryEntity, PostCategoryListItemModel>();

            CreateMap<PostCategoryView, PostCategoryListItemModel>();

            CreateMap<PostCategoryView, PostCategoryDetailsModel>();
        }
    }
}
