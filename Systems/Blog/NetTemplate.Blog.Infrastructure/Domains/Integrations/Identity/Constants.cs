using NetTemplate.Shared.Infrastructure.PubSub.Models;

namespace NetTemplate.Blog.Infrastructure.Domains.Integrations.Identity
{
    public static class Constants
    {
        public static class TopicNames
        {
            public const string IdentityUserCreated = nameof(IdentityUserCreated);
        }

        public static class ConfigurationSections
        {
            public static readonly string IdentityUserCreated = PubSubConfig.GetSection(TopicNames.IdentityUserCreated);
        }
    }
}
