namespace NetTemplate.Shared.ApplicationCore.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task ResetState(CancellationToken cancellationToken = default);
        Task<bool> CommitChanges(bool dispatchEvents = true, CancellationToken cancellationToken = default);
    }
}
