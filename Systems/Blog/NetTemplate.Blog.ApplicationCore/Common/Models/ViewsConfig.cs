using NetTemplate.Common.Objects.Interfaces;

namespace NetTemplate.Blog.ApplicationCore.Common.Models
{
    public class ViewsConfig : ICopyable<ViewsConfig>
    {
        public string PostViewVersion { get; set; }
        public string PostCategoryViewVersion { get; set; }

        public void CopyTo(ViewsConfig other)
        {
            other.PostViewVersion = PostViewVersion;
            other.PostCategoryViewVersion = PostCategoryViewVersion;
        }


        public const string ConfigurationSection = nameof(ViewsConfig);
    }
}
