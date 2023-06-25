using Hangfire;
using NetTemplate.Common.DependencyInjection.Attributes;
using NetTemplate.Shared.Infrastructure.Background.Extensions;
using NetTemplate.Shared.Infrastructure.Background.Interfaces;
using NetTemplate.Shared.Infrastructure.Background.Models;
using CrossJobNames = NetTemplate.Identity.ApplicationCore.Cross.Constants.JobNames;

namespace NetTemplate.Identity.Infrastructure.Common.Implementations
{
    [ScopedService]
    public class JobManager : IJobManager
    {
        private readonly IRecurringJobManager _recurringJobManager;

        public JobManager(IRecurringJobManager recurringJobManager)
        {
            _recurringJobManager = recurringJobManager;
        }

        public Task RunJobs(HangfireConfig config, CancellationToken cancellationToken = default)
        {
            CronJob[] jobs = config.Jobs?.Where(job => job.Enabled).ToArray();

            if (jobs?.Length > 0 != true) return Task.CompletedTask;

            TimeZoneInfo defaultTimeZone = config.TimeZoneInfo;

            foreach (var job in config.Jobs)
            {
                switch (job.Name)
                {
                    case CrossJobNames.Sample:
                        {
                            _recurringJobManager.ScheduleCronJob<object>(job,
                                (jobData) => () => Console.WriteLine(),
                                defaultTimeZone);
                            break;
                        }
                }
            }

            return Task.CompletedTask;
        }
    }
}
