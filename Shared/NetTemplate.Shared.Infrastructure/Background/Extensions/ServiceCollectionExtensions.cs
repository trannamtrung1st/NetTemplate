using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Common.DependencyInjection.Extensions;
using NetTemplate.Shared.Infrastructure.Background.Filters;
using NetTemplate.Shared.Infrastructure.Background.Models;
using NetTemplate.Shared.Infrastructure.Background.Utils;

namespace NetTemplate.Shared.Infrastructure.Background.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHangfireDefaults(this IServiceCollection services,
            HangfireConfig hangfireConfig, string connStr, string masterConnStr)
        {
            HangfireHelper.InitHangfireDatabase(masterConnStr, hangfireConfig.DatabaseName).Wait();

            services.AddHangfire(cfg =>
            {
                cfg.UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseFilter(new AutomaticRetryAttribute()
                    {
                        Attempts = hangfireConfig.RetryAttemps ?? DefaultRetryAttempts,
                        DelayInSecondsByAttemptFunc = (attempt) => (int)(hangfireConfig.RetrySecondsFactor ?? DefaultSecondsFactor * attempt)
                    })
                    .UseFilter(new JobLogging())
                    .UseSerilogLogProvider()

                    // [OPTIONAL] In-memory
                    //.UseInMemoryStorage()

                    // [IMPORTANT] SQL Server integration
                    .UseSqlServerStorage(connStr, new SqlServerStorageOptions
                    {
                        PrepareSchemaIfNecessary = true,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    })
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
            });

            if (hangfireConfig.StartServer)
            {
                services.AddHangfireServer(opt =>
                {
                    opt.ServerName = hangfireConfig.ServerName;

                    // [IMPORTANT] for job graceful shutdown
                    opt.CancellationCheckInterval = TimeSpan.FromSeconds(5);
                });
            }

            services.ConfigureCopyableConfig(hangfireConfig);

            return services;
        }

        public const int DefaultRetryAttempts = 3;
        public const int DefaultSecondsFactor = 2;
    }
}
