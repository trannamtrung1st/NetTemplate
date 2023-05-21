namespace NetTemplate.Blog.ApplicationCore.Post.Views
{
    public class PostView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public int? CreatorId { get; set; }
        public string CreatorName => $"{(FirstName != null ? $"{FirstName} " : "")}{LastName}";
        private string FirstName { get; set; }
        private string LastName { get; set; }
    }
}
