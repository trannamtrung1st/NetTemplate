using NetTemplate.Common.Objects.Interfaces;
using System.Reflection;

namespace NetTemplate.Blog.Infrastructure.Common.Models
{
    public class RuntimeConfig : ICopyable<RuntimeConfig>
    {
        public Assembly[] ScanningAssemblies { get; set; }

        public void CopyTo(RuntimeConfig other)
        {
            other.ScanningAssemblies = ScanningAssemblies;
        }
    }
}
