namespace NetTemplate.Common.Synchronization.Interfaces
{
    public interface ILockObject : IDisposable, IAsyncDisposable
    {
        string Resource { get; }
        string LockId { get; }
        bool IsAcquired { get; }
    }
}
