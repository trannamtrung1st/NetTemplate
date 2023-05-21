using AutoMapper;
using NetTemplate.Blog.ApplicationCore.Post.Models;
using NetTemplate.Blog.ApplicationCore.Post.Views;

namespace NetTemplate.Blog.ApplicationCore.Post.Mapping
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<PostEntity, PostView>()
                .ForMember(e => e.Tags, opt => opt.MapFrom(e => e.Tags.Select(t => t.Value).ToList()));

            CreateMap<PostEntity, PostListItemModel>();

            CreateMap<PostView, PostListItemModel>();
        }
    }
}
