using AutoMapper;
using NetTemplate.Blog.ApplicationCore.Common.Utils;
using NetTemplate.Blog.ApplicationCore.Post.Models;
using NetTemplate.Blog.ApplicationCore.Post.Views;
using static NetTemplate.Blog.ApplicationCore.PostCategory.Models.PostCategoryDetailsExtraModel;

namespace NetTemplate.Blog.ApplicationCore.Post.Mapping
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<PostEntity, BasePostResponseModel>()
                .ForMember(e => e.CreatorFullName, opt => opt.MapFrom(EntityHelper.GetCreatorFullNameExpression<PostEntity>()))
                .IncludeAllDerived();

            CreateMap<PostEntity, PostListItemModel>();

            CreateMap<PostEntity, PostView>()
                .ForMember(e => e.Tags, opt => opt.MapFrom(src => src.Tags.Select(tag => tag.Value).ToList()));

            CreateMap<PostView, PostDetailsModel>();

            CreateMap<PostEntity, PostDetailsModel>()
                .ForMember(e => e.Tags, opt => opt.MapFrom(src => src.Tags.Select(tag => tag.Value).ToList()));

            CreateMap<PostEntity, LatestPostModel>();
        }
    }
}
