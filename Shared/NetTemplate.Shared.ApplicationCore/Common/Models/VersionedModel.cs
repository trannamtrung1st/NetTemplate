namespace NetTemplate.Shared.ApplicationCore.Common.Models
{
    public interface IVersionedModel
    {
        string _version_ { get; }
    }

    public abstract class VersionedModel : IVersionedModel
    {
        public string _version_ { get; set; }
    }
}
