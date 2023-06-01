using AutoMapper;
using NetTemplate.Blog.ApplicationCore.User.Models;
using NetTemplate.Blog.ApplicationCore.User.Views;

namespace NetTemplate.Blog.ApplicationCore.User.Mapping
{
    public class UserPartialProfile : Profile
    {
        public UserPartialProfile()
        {
            CreateMap<UserPartialEntity, UserView>();

            CreateMap<UserPartialEntity, UserListItemModel>()
                .ForMember(e => e.FullName, opt => opt.MapFrom(UserPartialEntity.FullNameExpression));
        }
    }
}
