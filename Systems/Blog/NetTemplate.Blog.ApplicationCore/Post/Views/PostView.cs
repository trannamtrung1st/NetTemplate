namespace NetTemplate.Blog.ApplicationCore.Post.Views
{
    public class PostView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public PostCategoryView Category { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public int? CreatorId { get; set; }
        public string CreatorName { get; set; }
    }
}
