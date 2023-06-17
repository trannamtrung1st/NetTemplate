using NetTemplate.Common.Synchronization.Interfaces;
using RedLockNet;

namespace NetTemplate.Redis.Implementations
{
    public class RedisRedLockObject : ILockObject
    {
        private readonly IRedLock _redLock;

        public RedisRedLockObject(IRedLock redLock)
        {
            _redLock = redLock;
        }

        public string Resource => _redLock.Resource;

        public string LockId => _redLock.LockId;

        public bool IsAcquired => _redLock.IsAcquired;

        public void Dispose()
        {
            _redLock?.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            return _redLock?.DisposeAsync() ?? ValueTask.CompletedTask;
        }
    }
}
