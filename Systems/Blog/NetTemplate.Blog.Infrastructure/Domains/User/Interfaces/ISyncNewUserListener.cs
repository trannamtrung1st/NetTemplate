using NetTemplate.Shared.Infrastructure.PubSub.Interfaces;

namespace NetTemplate.Blog.Infrastructure.Domains.User.Interfaces
{
    public interface ISyncNewUserListener : ITopicListener
    {
    }
}
