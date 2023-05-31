namespace NetTemplate.Blog.ApplicationCore.Comment.Models
{
    public abstract class BaseCommentResponseModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string CreatorUserCode { get; set; }
        public string CreatorFullName { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
