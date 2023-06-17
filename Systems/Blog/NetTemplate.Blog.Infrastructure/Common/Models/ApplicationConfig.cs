using NetTemplate.Common.Objects.Interfaces;

namespace NetTemplate.Blog.Infrastructure.Common.Models
{
    public class ApplicationConfig : ICopyable<ApplicationConfig>
    {
        public bool DbContextDebugEnabled { get; set; }

        public void CopyTo(ApplicationConfig other)
        {
            other.DbContextDebugEnabled = DbContextDebugEnabled;
        }


        public const string ConfigurationSection = nameof(ApplicationConfig);
    }
}
