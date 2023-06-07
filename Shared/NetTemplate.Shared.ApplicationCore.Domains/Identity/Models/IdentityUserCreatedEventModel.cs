namespace NetTemplate.Shared.ApplicationCore.Domains.Identity.Models
{
    public class IdentityUserCreatedEventModel
    {
        public string UserCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
    }
}
