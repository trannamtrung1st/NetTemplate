namespace NetTemplate.Shared.ApplicationCore.Common.Models
{
    public interface IVersionedModel
    {
        string Version { get; }
    }

    public abstract class VersionedModel
    {
        public string @Version { get; protected set; }

        public virtual void SetVersion(string version)
        {
            @Version = version;
        }
    }
}
