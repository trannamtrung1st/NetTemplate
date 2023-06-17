using NetTemplate.Common.Synchronization.Interfaces;

namespace NetTemplate.Common.Synchronization.Implementations
{
    public class SemaphoreSlimLockObject : ILockObject
    {
        private readonly SemaphoreSlim _semaphoreSlim;
        private bool disposedValue;

        public SemaphoreSlimLockObject(
            SemaphoreSlim semaphoreSlim,
            string resource, string lockId, bool isAcquired)
        {
            _semaphoreSlim = semaphoreSlim;
            Resource = resource;
            LockId = lockId;
            IsAcquired = isAcquired;
        }

        public string Resource { get; }

        public string LockId { get; }

        public bool IsAcquired { get; }

        public ValueTask DisposeAsync()
        {
            Dispose();

            return ValueTask.CompletedTask;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _semaphoreSlim?.Release();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
