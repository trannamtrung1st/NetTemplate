namespace NetTemplate.Blog.ApplicationCore.Post.Views
{
    public class PostCategoryView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CreatorId { get; set; }
        public string CreatorName { get; set; }
    }
}
