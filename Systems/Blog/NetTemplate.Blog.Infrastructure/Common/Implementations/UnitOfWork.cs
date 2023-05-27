using NetTemplate.Blog.Infrastructure.Persistence;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.Infrastructure.Common.Implementations
{
    [ScopedService]
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MainDbContext _mainDbContext;

        public UnitOfWork(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public Task ResetState()
        {
            return _mainDbContext.ResetState();
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
