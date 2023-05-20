namespace NetTemplate.Blog.ApplicationCore.Post.Models
{
    public class UpdatePostTagsModel
    {
        public int Id { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
