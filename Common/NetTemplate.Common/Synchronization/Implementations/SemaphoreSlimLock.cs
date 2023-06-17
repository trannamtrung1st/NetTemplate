using NetTemplate.Common.Synchronization.Interfaces;
using System.Collections.Concurrent;

namespace NetTemplate.Common.Synchronization.Implementations
{
    public class SemaphoreSlimLock : IDistributedLock
    {
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _lockMap;

        public SemaphoreSlimLock()
        {
            _lockMap = new ConcurrentDictionary<string, SemaphoreSlim>();
        }

        public Task<ILockObject> CreateLock(string resource, TimeSpan expiryTime, TimeSpan waitTime, TimeSpan retryTime, CancellationToken? cancellationToken = null)
        {
            SemaphoreSlim semaphore = _lockMap.GetOrAdd(resource, (_) => new SemaphoreSlim(1));

            bool acquired = semaphore.Wait(waitTime, cancellationToken ?? default);

            ILockObject lockObj = new SemaphoreSlimLockObject(
                semaphoreSlim: acquired ? semaphore : null,
                resource: resource,
                lockId: resource,
                acquired);

            return Task.FromResult(lockObj);
        }
    }
}
