namespace NetTemplate.Blog.ApplicationCore.PostCategory.Models
{
    public class PostCategoryDetailsExtraModel
    {
        public LatestPostModel LatestPost { get; set; }
        public int PostCount { get; set; }

        public class LatestPostModel
        {
            public int Id { get; set; }
            public string Title { get; set; }
        }
    }
}
