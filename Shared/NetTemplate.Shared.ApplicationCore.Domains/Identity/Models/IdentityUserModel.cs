namespace NetTemplate.Shared.ApplicationCore.Domains.Identity.Models
{
    public class IdentityUserModel
    {
        public string UserCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
    }
}
