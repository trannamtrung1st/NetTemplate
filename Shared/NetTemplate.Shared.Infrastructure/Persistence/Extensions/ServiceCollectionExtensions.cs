using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NetTemplate.Shared.Infrastructure.Persistence.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContextDefaults<T>(this IServiceCollection services,
            string connStr, bool debugEnabled) where T : DbContext
        {
            if (connStr is null) throw new ArgumentNullException(nameof(connStr));

            services.AddDbContext<T>((provider, options) =>
            {
                options
                    .UseSqlServer(connStr)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);

                if (debugEnabled)
                {
                    var loggerFactory = LoggerFactory.Create(factory =>
                        factory.AddConsole());

                    options
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors()
                        .UseLoggerFactory(loggerFactory);
                }
            });

            return services;
        }
    }
}
