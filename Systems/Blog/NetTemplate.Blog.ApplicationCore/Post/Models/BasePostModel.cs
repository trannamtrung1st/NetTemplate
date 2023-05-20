namespace NetTemplate.Blog.ApplicationCore.Post.Models
{
    public abstract class BasePostModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}
