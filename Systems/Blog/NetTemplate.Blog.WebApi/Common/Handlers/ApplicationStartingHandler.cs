using MediatR;
using NetTemplate.Blog.Infrastructure.Common.Interfaces;
using NetTemplate.Blog.Infrastructure.Persistence;
using NetTemplate.Shared.ApplicationCore.Common.Events;
using NetTemplate.Shared.Infrastructure.Background.Models;
using NetTemplate.Shared.Infrastructure.PubSub.Models;

namespace NetTemplate.Blog.WebApi.Common.Handlers
{
    public class ApplicationStartingHandler : INotificationHandler<ApplicationStartingEvent>
    {
        private readonly MainDbContext _dbContext;
        private readonly IServiceProvider _provider;
        private readonly IJobManager _jobManager;
        private readonly IConsumerManager _consumerManager;

        public ApplicationStartingHandler(
            MainDbContext dbContext,
            IServiceProvider provider,
            IJobManager jobManager,
            IConsumerManager consumerManager)
        {
            _dbContext = dbContext;
            _provider = provider;
            _jobManager = jobManager;
            _consumerManager = consumerManager;
        }

        public async Task Handle(ApplicationStartingEvent @event, CancellationToken cancellationToken)
        {
            HangfireConfig hangfireConfig = @event.Data.HangfireConfig;
            PubSubConfig pubSubConfig = @event.Data.PubSubConfig;

            await MigrateDatabase(cancellationToken);

            await RunJobs(hangfireConfig, cancellationToken);

            await StartConsumers(pubSubConfig, cancellationToken);
        }

        private async Task MigrateDatabase(CancellationToken cancellationToken = default)
        {
            await _dbContext.Migrate(_provider, cancellationToken);
        }

        private async Task RunJobs(HangfireConfig config, CancellationToken cancellationToken = default)
        {
            await _jobManager.RunJobs(config, cancellationToken);
        }

        private async Task StartConsumers(PubSubConfig pubSubConfig, CancellationToken cancellationToken = default)
        {
            await _consumerManager.StartConsumers(pubSubConfig, cancellationToken);
        }
    }
}
