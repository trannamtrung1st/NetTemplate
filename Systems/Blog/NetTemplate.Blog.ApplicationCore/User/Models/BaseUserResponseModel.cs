namespace NetTemplate.Blog.ApplicationCore.User.Models
{
    public abstract class BaseUserResponseModel
    {
        public int Id { get; set; }
        public string UserCode { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string FullName { get; private set; }
        public bool Active { get; private set; }
    }
}
