using Hangfire;
using NetTemplate.Blog.ApplicationCore.User.Jobs.SyncUsers;
using NetTemplate.Blog.Infrastructure.Domains.User.JobRunners;
using NetTemplate.Common.DependencyInjection;
using NetTemplate.Shared.Infrastructure.Background.Extensions;
using NetTemplate.Shared.Infrastructure.Background.Interfaces;
using NetTemplate.Shared.Infrastructure.Background.Models;
using CommonJobNames = NetTemplate.Blog.ApplicationCore.Cross.Constants.JobNames;
using UserJobNames = NetTemplate.Blog.ApplicationCore.User.Constants.JobNames;

namespace NetTemplate.Blog.Infrastructure.Common.Implementations
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
                    case CommonJobNames.Sample:
                        {
                            _recurringJobManager.ScheduleCronJob<object>(job,
                                (jobData) => () => Console.WriteLine(),
                                defaultTimeZone);
                            break;
                        }
                    case UserJobNames.SyncUsers:
                        {
                            _recurringJobManager.ScheduleCronJob<ISyncUsersJobRunner, SyncUsersJobArgument>(job,
                                (jobData) => (service) => service.Start(jobData, default),
                                defaultTimeZone);
                            break;
                        }
                }
            }

            return Task.CompletedTask;
        }
    }
}
