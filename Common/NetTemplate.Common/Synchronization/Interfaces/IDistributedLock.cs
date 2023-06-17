namespace NetTemplate.Common.Synchronization.Interfaces
{
    public interface IDistributedLock
    {
        Task<ILockObject> CreateLock(string resource, TimeSpan expiryTime, TimeSpan waitTime, TimeSpan retryTime, CancellationToken? cancellationToken = null);
    }
}
