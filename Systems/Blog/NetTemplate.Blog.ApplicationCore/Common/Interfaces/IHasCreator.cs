using NetTemplate.Blog.ApplicationCore.User;

namespace NetTemplate.Blog.ApplicationCore.Common.Interfaces
{
    public interface IHasCreator
    {
        UserPartialEntity Creator { get; }
    }
}
