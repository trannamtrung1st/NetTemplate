using NetTemplate.Shared.Infrastructure.Background.Models;

namespace NetTemplate.Blog.Infrastructure.Common.Interfaces
{
    public interface IJobManager
    {
        Task RunJobs(HangfireConfig hangfireConfig, CancellationToken cancellationToken = default);
    }
}
