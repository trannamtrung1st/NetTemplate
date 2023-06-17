using Hangfire.Common;
using Hangfire.Logging;
using Hangfire.Server;
using NetTemplate.Common.Logging.Extensions;
using NetTemplate.Common.Reflection.Extensions;

namespace NetTemplate.Shared.Infrastructure.Background.Filters
{
    public class JobLogging : JobFilterAttribute, IServerFilter, IServerExceptionFilter
    {
        private readonly ILog _logger;

        public JobLogging()
        {
            _logger = LogProvider.GetCurrentClassLogger();
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            var backgroundJob = filterContext.BackgroundJob;
            _logger.InfoFormat("[END] Job {0}:{1}, method: {2}",
                backgroundJob.Id,
                backgroundJob.Job.Type.GetGenericTypeName(),
                backgroundJob.Job.Method.GetDescription(backgroundJob.Job.Args.ToArray()));
        }

        public void OnPerforming(PerformingContext context)
        {
            var backgroundJob = context.BackgroundJob;
            _logger.InfoFormat("[START] Job {0}:{1}, method: {2}",
                backgroundJob.Id,
                backgroundJob.Job.Type.GetGenericTypeName(),
                backgroundJob.Job.Method.GetDescription(backgroundJob.Job.Args.ToArray()));
        }

        public void OnServerException(ServerExceptionContext filterContext)
        {
            var backgroundJob = filterContext.BackgroundJob;
            _logger.ErrorException($"[ERROR] Job {backgroundJob.Id}", filterContext.Exception);
        }
    }
}
