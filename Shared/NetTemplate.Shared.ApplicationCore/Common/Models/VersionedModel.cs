using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Shared.ApplicationCore.Common.Models
{
    public abstract class VersionedModel : IVersionedModel
    {
        public string _version_ { get; set; }
    }
}
