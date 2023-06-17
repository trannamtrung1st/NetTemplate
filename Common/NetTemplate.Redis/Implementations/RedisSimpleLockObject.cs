using NetTemplate.Common.Synchronization.Interfaces;

namespace NetTemplate.Redis.Implementations
{
    public class RedisSimpleLockObject : ILockObject
    {
        private readonly Func<Task> Release;
        private bool disposedValue;

        public RedisSimpleLockObject(
            Func<Task> Release,
            string resource, string lockId, bool isAcquired)
        {
            this.Release = Release;
            Resource = resource;
            LockId = lockId;
            IsAcquired = isAcquired;
        }

        public string Resource { get; }

        public string LockId { get; }

        public bool IsAcquired { get; }

        public async ValueTask DisposeAsync()
        {
            if (!disposedValue)
            {
                if (Release != null) await Release();

                disposedValue = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Release != null) Release().Wait();
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
