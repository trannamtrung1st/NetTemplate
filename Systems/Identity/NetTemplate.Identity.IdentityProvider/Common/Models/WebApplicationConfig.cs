using NetTemplate.Common.Objects.Interfaces;
using NetTemplate.Identity.Infrastructure.Common.Models;

namespace NetTemplate.Identity.IdentityProvider.Common.Models
{
    public class WebApplicationConfig : ApplicationConfig, ICopyable<WebApplicationConfig>
    {
        public int ResponseCacheTtl { get; set; }

        public void CopyTo(WebApplicationConfig other)
        {
            base.CopyTo(other);
            other.ResponseCacheTtl = ResponseCacheTtl;
        }
    }
}
