namespace NetTemplate.Shared.ApplicationCore.Interfaces.Services
{
    public interface IUnitOfWork : IDisposable
    {
        Task ResetStateAsync();

        Task<bool> SaveEntitiesAsync(bool dispatchEvents = true, CancellationToken cancellationToken = default);
    }
}
