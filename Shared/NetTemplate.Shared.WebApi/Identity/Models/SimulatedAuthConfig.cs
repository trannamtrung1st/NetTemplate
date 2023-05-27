namespace NetTemplate.Shared.WebApi.Identity.Models
{
    public class SimulatedAuthConfig
    {
        public bool Enabled { get; set; }
        public int UserId { get; set; }
        public string UserCode { get; set; }
        public IDictionary<string, string[]> Claims { get; set; }
    }
}
