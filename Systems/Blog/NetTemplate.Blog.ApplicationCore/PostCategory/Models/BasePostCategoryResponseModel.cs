namespace NetTemplate.Blog.ApplicationCore.PostCategory.Models
{
    public abstract class BasePostCategoryResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CreatorId { get; set; }
        public string CreatorUserCode { get; set; }
        public string CreatorFullName { get; set; }
    }
}
