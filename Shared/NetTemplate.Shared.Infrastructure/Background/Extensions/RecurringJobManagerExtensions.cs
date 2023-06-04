using Hangfire;
using NetTemplate.Shared.Infrastructure.Background.Models;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace NetTemplate.Shared.Infrastructure.Background.Extensions
{
    public static class RecurringJobManagerExtensions
    {
        public static IRecurringJobManager ScheduleCronJob<TService, TData>(this IRecurringJobManager manager,
            CronJob job, Func<TData, Expression<Func<TService, Task>>> GetJob, TimeZoneInfo defaultTimeZone)
        {
            return manager.ScheduleCronJob(job, GetJobTask: GetJob, GetJobAction: null, defaultTimeZone);
        }

        public static IRecurringJobManager ScheduleCronJob<TService, TData>(this IRecurringJobManager manager,
            CronJob job, Func<TData, Expression<Action<TService>>> GetJob, TimeZoneInfo defaultTimeZone)
        {
            return manager.ScheduleCronJob(job, GetJobTask: null, GetJobAction: GetJob, defaultTimeZone);
        }

        public static IRecurringJobManager ScheduleCronJob<TData>(this IRecurringJobManager manager,
            CronJob job, Func<TData, Expression<Func<Task>>> GetJob, TimeZoneInfo defaultTimeZone)
        {
            return manager.ScheduleCronJob(job, GetJobTask: GetJob, GetJobAction: null, defaultTimeZone);
        }

        public static IRecurringJobManager ScheduleCronJob<TData>(this IRecurringJobManager manager,
            CronJob job, Func<TData, Expression<Action>> GetJob, TimeZoneInfo defaultTimeZone)
        {
            return manager.ScheduleCronJob(job, GetJobTask: null, GetJobAction: GetJob, defaultTimeZone);
        }

        private static IRecurringJobManager ScheduleCronJob<TService, TData>(
            this IRecurringJobManager manager, CronJob job,
            Func<TData, Expression<Func<TService, Task>>> GetJobTask,
            Func<TData, Expression<Action<TService>>> GetJobAction,
            TimeZoneInfo defaultTimeZone)
        {
            string serializedData = JsonConvert.SerializeObject(job.JobData);
            TData jobData = JsonConvert.DeserializeObject<TData>(serializedData);
            int count = 1;

            foreach (var cronExpr in job.CronExpressions)
            {
                var finalName = job.Name + (count++);

                if (GetJobTask != null)
                {
                    manager.AddOrUpdate(finalName,
                        GetJobTask(jobData),
                        cronExpr, new RecurringJobOptions { TimeZone = defaultTimeZone });
                }
                else
                {
                    manager.AddOrUpdate(finalName,
                        GetJobAction(jobData),
                        cronExpr, new RecurringJobOptions { TimeZone = defaultTimeZone });
                }
            }

            return manager;
        }

        private static IRecurringJobManager ScheduleCronJob<TData>(
            this IRecurringJobManager manager, CronJob job,
            Func<TData, Expression<Func<Task>>> GetJobTask,
            Func<TData, Expression<Action>> GetJobAction,
            TimeZoneInfo defaultTimeZone)
        {
            string serializedData = JsonConvert.SerializeObject(job.JobData);
            TData jobData = JsonConvert.DeserializeObject<TData>(serializedData);
            int count = 1;

            foreach (var cronExpr in job.CronExpressions)
            {
                var finalName = job.Name + (count++);

                if (GetJobTask != null)
                {
                    manager.AddOrUpdate(finalName,
                        GetJobTask(jobData),
                        cronExpr, new RecurringJobOptions { TimeZone = defaultTimeZone });
                }
                else
                {
                    manager.AddOrUpdate(finalName,
                        GetJobAction(jobData),
                        cronExpr, new RecurringJobOptions { TimeZone = defaultTimeZone });
                }
            }

            return manager;
        }
    }
}
