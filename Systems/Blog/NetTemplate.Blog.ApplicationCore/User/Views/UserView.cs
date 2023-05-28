namespace NetTemplate.Blog.ApplicationCore.User.Views
{
    public class UserView
    {
        public int Id { get; set; }
        public string UserCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}
