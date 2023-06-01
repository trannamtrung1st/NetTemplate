using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTemplate.Blog.Infrastructure.Persistence;
using NetTemplate.Shared.ApplicationCore.Common.Events;
using NetTemplate.Shared.Infrastructure.Background.Models;
using Newtonsoft.Json;
using CommonJobNames = NetTemplate.Blog.ApplicationCore.Common.Constants.JobNames;

namespace NetTemplate.Blog.WebApi.Common.Handlers
{
    public class ApplicationStartingHandler : INotificationHandler<ApplicationStartingEvent>
    {
        private readonly MainDbContext _dbContext;
        private readonly IServiceProvider _provider;
        private readonly IRecurringJobManager _recurringJobManager;

        public ApplicationStartingHandler(
            MainDbContext dbContext,
            IServiceProvider provider,
            IRecurringJobManager recurringJobManager)
        {
            _dbContext = dbContext;
            _provider = provider;
            _recurringJobManager = recurringJobManager;
        }

        public async Task Handle(ApplicationStartingEvent @event, CancellationToken cancellationToken)
        {
            HangfireConfig hangfireConfig = @event.Data.HangfireConfig;

            await MigrateDatabase(cancellationToken);

            await RunJobs(hangfireConfig, cancellationToken);

            await StartConsumers(cancellationToken);
        }

        private async Task MigrateDatabase(CancellationToken cancellationToken = default)
        {
            await _dbContext.Database.MigrateAsync(cancellationToken);
            await _dbContext.SeedMigrationsAsync(_provider, cancellationToken);
        }

        private Task RunJobs(HangfireConfig config, CancellationToken cancellationToken = default)
        {
            IEnumerable<CronJob> jobs = config.Jobs;

            if (jobs?.Any() != true) return Task.CompletedTask;

            TimeZoneInfo defaultTimeZone = config.TimeZoneInfo;

            foreach (var job in config.Jobs)
            {
                var count = 1;
                foreach (var cronExpr in job.CronExpressions)
                {
                    switch (job.Name)
                    {
                        case CommonJobNames.Sample:
                            {
                                var serializedData = JsonConvert.SerializeObject(job.JobData);
                                var jobData = JsonConvert.DeserializeObject(serializedData);
                                var finalName = CommonJobNames.Sample + (count++);

                                _recurringJobManager.AddOrUpdate(finalName,
                                    () => Console.WriteLine("Sample Job Run"),
                                    cronExpr,
                                    new RecurringJobOptions { TimeZone = defaultTimeZone });
                                break;
                            }
                    }
                }
            }

            return Task.CompletedTask;
        }

        private Task StartConsumers(CancellationToken cancellationToken = default)
        {
            // [TODO]

            return Task.CompletedTask;
        }
    }
}
