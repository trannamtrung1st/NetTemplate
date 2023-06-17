using Hangfire.Common;
using Hangfire.Logging;
using Hangfire.Server;
using NetTemplate.Common.Logging.Extensions;
using NetTemplate.Common.Reflection.Extensions;

namespace NetTemplate.Shared.Infrastructure.Background.Filters
{
    public class IgnoreIfTimedOut : JobFilterAttribute, IServerFilter
    {
        public const int DefaultTimeOutMs = 60000;

        private readonly int _timeOutMs;
        private readonly ILog _logger;

        public IgnoreIfTimedOut(int timeOutMs = DefaultTimeOutMs)
        {
            _logger = LogProvider.GetCurrentClassLogger();
            _timeOutMs = timeOutMs;
        }

        public void OnPerformed(PerformedContext filterContext)
        {
        }

        public void OnPerforming(PerformingContext context)
        {
            bool misfired = (DateTime.UtcNow - context.BackgroundJob.CreatedAt).TotalMilliseconds > _timeOutMs;

            if (misfired)
            {
                var backgroundJob = context.BackgroundJob;
                _logger.InfoFormat("[CANCELLING] Job {0}:{1}, method: {2}",
                    backgroundJob.Id,
                    backgroundJob.Job.Type.GetGenericTypeName(),
                    backgroundJob.Job.Method.GetDescription(backgroundJob.Job.Args.ToArray()));
                context.Canceled = true;
            }
        }
    }
}
