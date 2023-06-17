using NetTemplate.Common.Synchronization.Interfaces;
using RedLockNet;
using RedLockNet.SERedis;

namespace NetTemplate.Redis.Implementations
{
    public class RedisRedLock : IDistributedLock
    {
        private readonly RedLockFactory _redLockFactory;

        public RedisRedLock(RedLockFactory redLockFactory)
        {
            _redLockFactory = redLockFactory;
        }

        public async Task<ILockObject> CreateLock(string resource, TimeSpan expiryTime, TimeSpan waitTime, TimeSpan retryTime, CancellationToken? cancellationToken = null)
        {
            IRedLock redLock = await _redLockFactory.CreateLockAsync(resource, expiryTime, waitTime, retryTime, cancellationToken);

            return new RedisRedLockObject(redLock);
        }
    }
}
