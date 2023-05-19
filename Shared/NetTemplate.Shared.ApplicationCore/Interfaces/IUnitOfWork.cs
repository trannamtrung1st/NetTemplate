namespace NetTemplate.Shared.ApplicationCore.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task ResetState();
        Task<bool> CommitChanges(bool dispatchEvents = true, CancellationToken cancellationToken = default);
    }
}
