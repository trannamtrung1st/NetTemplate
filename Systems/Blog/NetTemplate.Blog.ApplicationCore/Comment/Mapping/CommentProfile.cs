using AutoMapper;
using NetTemplate.Blog.ApplicationCore.Comment.Models;
using NetTemplate.Blog.ApplicationCore.Common.Utils;

namespace NetTemplate.Blog.ApplicationCore.Comment.Mapping
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<CommentEntity, BaseCommentResponseModel>()
                .ForMember(e => e.CreatorFullName, opt => opt.MapFrom(EntityHelper.GetCreatorFullNameExpression<CommentEntity>()))
                .IncludeAllDerived();

            CreateMap<CommentEntity, CommentListItemModel>();

            CreateMap<CommentEntity, CommentDetailsModel>();
        }
    }
}
