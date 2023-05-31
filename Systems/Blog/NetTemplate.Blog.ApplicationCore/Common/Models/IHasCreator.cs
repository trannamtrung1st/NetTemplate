using NetTemplate.Blog.ApplicationCore.User;

namespace NetTemplate.Blog.ApplicationCore.Common.Models
{
    public interface IHasCreator
    {
        UserPartialEntity Creator { get; }
    }
}
