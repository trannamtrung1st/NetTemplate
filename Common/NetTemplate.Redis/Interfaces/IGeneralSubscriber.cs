namespace NetTemplate.Redis.Interfaces
{
    public interface IGeneralSubscriber
    {
        bool Enabled { get; }
        Task Start(CancellationToken cancellationToken = default);
    }
}
