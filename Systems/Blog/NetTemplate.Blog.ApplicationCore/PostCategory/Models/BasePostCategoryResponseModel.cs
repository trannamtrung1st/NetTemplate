namespace NetTemplate.Blog.ApplicationCore.PostCategory.Models
{
    public abstract class BasePostCategoryResponseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
