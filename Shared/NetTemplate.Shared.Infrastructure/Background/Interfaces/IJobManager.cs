using NetTemplate.Shared.Infrastructure.Background.Models;

namespace NetTemplate.Shared.Infrastructure.Background.Interfaces
{
    public interface IJobManager
    {
        Task RunJobs(HangfireConfig hangfireConfig, CancellationToken cancellationToken = default);
    }
}
