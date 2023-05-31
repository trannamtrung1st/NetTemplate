using AutoMapper;
using NetTemplate.Blog.ApplicationCore.Common.Utils;
using NetTemplate.Blog.ApplicationCore.PostCategory.Models;
using NetTemplate.Blog.ApplicationCore.PostCategory.Views;

namespace NetTemplate.Blog.ApplicationCore.PostCategory.Mapping
{
    public class PostCategoryProfile : Profile
    {
        public PostCategoryProfile()
        {
            CreateMap<PostCategoryEntity, BasePostCategoryResponseModel>()
                .ForMember(e => e.CreatorFullName, opt => opt.MapFrom(EntityHelper.GetCreatorFullNameExpression<PostCategoryEntity>()))
                .IncludeAllDerived();

            CreateMap<PostCategoryEntity, PostCategoryListItemModel>();

            CreateMap<PostCategoryEntity, PostCategoryView>();

            CreateMap<PostCategoryEntity, Post.Views.PostCategoryView>();

            CreateMap<PostCategoryView, PostCategoryListItemModel>();

            CreateMap<PostCategoryView, PostCategoryDetailsModel>();
        }
    }
}
