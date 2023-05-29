namespace NetTemplate.Blog.ApplicationCore.Post.Models
{
    public class PostDetailsModel : BasePostResponseModel
    {
        public string Content { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
