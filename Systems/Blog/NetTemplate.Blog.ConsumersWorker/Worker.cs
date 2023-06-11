using NetTemplate.Shared.Infrastructure.PubSub.Interfaces;

namespace NetTemplate.Blog.ConsumersWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ITopicListenerManager _consumerManager;
        private readonly ITopicManager _topicManager;

        public Worker(ILogger<Worker> logger,
            ITopicListenerManager consumerManager,
            ITopicManager topicManager)
        {
            _logger = logger;
            _consumerManager = consumerManager;
            _topicManager = topicManager;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await UpdateTopics(cancellationToken);

            await StartListeners(cancellationToken);
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