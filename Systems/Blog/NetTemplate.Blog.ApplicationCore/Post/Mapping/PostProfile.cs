using AutoMapper;
using NetTemplate.Blog.ApplicationCore.Post.Models;
using NetTemplate.Blog.ApplicationCore.Post.Views;

namespace NetTemplate.Blog.ApplicationCore.Post.Mapping
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<PostEntity, BasePostResponseModel>()
                .ForMember(e => e.CreatorFullName, opt => opt.MapFrom(PostEntity.CreatorFullNameExpression))
                .IncludeAllDerived();

            CreateMap<PostEntity, PostListItemModel>();

            CreateMap<PostEntity, PostView>()
                .ForMember(e => e.Tags, opt => opt.MapFrom(src => src.Tags.Select(tag => tag.Value).ToList()));

            CreateMap<PostView, PostDetailsModel>();

            CreateMap<PostEntity, PostDetailsModel>()
                .ForMember(e => e.Tags, opt => opt.MapFrom(src => src.Tags.Select(tag => tag.Value).ToList()));
        }
    }
}
