using NetTemplate.Blog.ApplicationCore.User.Views;

namespace NetTemplate.Blog.ApplicationCore.Post.Views
{
    public class PostView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? CreatorId { get; set; }
        public UserView Creator { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
