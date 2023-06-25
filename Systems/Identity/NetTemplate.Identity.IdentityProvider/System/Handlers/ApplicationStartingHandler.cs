using MediatR;
using Microsoft.Extensions.Options;
using NetTemplate.Identity.Infrastructure.Persistence;
using NetTemplate.Shared.ApplicationCore.Common.Events;
using NetTemplate.Shared.Infrastructure.Background.Interfaces;
using NetTemplate.Shared.Infrastructure.Background.Models;
using NetTemplate.Shared.Infrastructure.PubSub.Interfaces;

namespace NetTemplate.Identity.IdentityProvider.System.Handlers
{
    public class ApplicationStartingHandler : INotificationHandler<ApplicationStartingEvent>
    {
        private readonly MainDbContext _dbContext;
        private readonly IServiceProvider _provider;
        private readonly IJobManager _jobManager;
        private readonly ITopicListenerManager _consumerManager;
        private readonly ITopicManager _topicManager;
        private readonly IOptions<HangfireConfig> _hangfireOptions;

        public ApplicationStartingHandler(
            MainDbContext dbContext,
            IServiceProvider provider,
            IJobManager jobManager,
            ITopicListenerManager consumerManager,
            ITopicManager topicManager,
            IOptions<HangfireConfig> hangfireOptions)
        {
            _dbContext = dbContext;
            _provider = provider;
            _jobManager = jobManager;
            _consumerManager = consumerManager;
            _topicManager = topicManager;
            _hangfireOptions = hangfireOptions;
        }

        public async Task Handle(ApplicationStartingEvent @event, CancellationToken cancellationToken)
        {
            await MigrateDatabase(cancellationToken);

            await UpdateTopics(cancellationToken);

            await Task.WhenAll(
                RunJobs(cancellationToken),
                StartListeners(cancellationToken));
        }

        private async Task MigrateDatabase(CancellationToken cancellationToken = default)
        {
            await _dbContext.Migrate(_provider, cancellationToken);
        }

        private async Task RunJobs(CancellationToken cancellationToken = default)
        {
            await _jobManager.RunJobs(_hangfireOptions.Value, cancellationToken);
        }

        private async Task UpdateTopics(CancellationToken cancellationToken = default)
        {
            await _topicManager.UpdateTopics(cancellationToken);
        }

        private async Task StartListeners(CancellationToken cancellationToken = default)
        {
            await _consumerManager.StartListeners(cancellationToken);
        }
    }
}
