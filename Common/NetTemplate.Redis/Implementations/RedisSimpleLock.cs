using NetTemplate.Common.Synchronization.Interfaces;
using StackExchange.Redis;

namespace NetTemplate.Redis.Implementations
{
    public class RedisSimpleLock : IDistributedLock
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public RedisSimpleLock(ConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<ILockObject> CreateLock(string resource, TimeSpan expiryTime, TimeSpan waitTime, TimeSpan retryTime, CancellationToken? cancellationToken = null)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            string lockValue = Guid.NewGuid().ToString();
            DateTime waitUntil = DateTime.UtcNow.Add(waitTime);
            bool acquired = false;

            do
            {
                acquired = await db.LockTakeAsync(resource, lockValue, expiryTime);

                if (!acquired)
                {
                    await Task.Delay(retryTime);
                }
            } while (!acquired && DateTime.UtcNow < waitUntil);

            return new RedisSimpleLockObject(
                Release: acquired ? () => db.LockReleaseAsync(resource, lockValue) : null,
                resource: resource,
                lockId: lockValue,
                acquired);
        }
    }
}
