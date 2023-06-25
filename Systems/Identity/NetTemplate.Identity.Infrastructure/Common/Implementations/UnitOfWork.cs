using NetTemplate.Common.DependencyInjection.Attributes;
using NetTemplate.Identity.Infrastructure.Persistence;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Identity.Infrastructure.Common.Implementations
{
    [ScopedService]
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MainDbContext _mainDbContext;

        public UnitOfWork(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public Task ResetState(CancellationToken cancellationToken = default)
        {
            return _mainDbContext.ResetState(cancellationToken);
        }

        public async Task<bool> CommitChanges(bool dispatchEvents = true, CancellationToken cancellationToken = default)
        {
            await _mainDbContext.SaveEntitiesAsync(dispatchEvents, cancellationToken);

            return true;
        }

        public void Dispose()
        {
            _mainDbContext.Dispose();
        }
    }
}
