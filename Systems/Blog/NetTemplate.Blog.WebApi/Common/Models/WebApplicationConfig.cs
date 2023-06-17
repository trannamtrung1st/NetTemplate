using NetTemplate.Blog.Infrastructure.Common.Models;
using NetTemplate.Common.Objects.Interfaces;

namespace NetTemplate.Blog.WebApi.Common.Models
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
