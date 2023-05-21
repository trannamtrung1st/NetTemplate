namespace NetTemplate.Blog.ApplicationCore.Post.Models
{
    public class PostDetailsModel : BasePostResponseModel
    {
        public IEnumerable<string> Tags { get; set; }
    }
}
