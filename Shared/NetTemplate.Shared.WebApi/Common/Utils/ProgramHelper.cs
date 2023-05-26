using Autofac.Extensions.DependencyInjection;
using NetTemplate.Shared.Infrastructure.Logging.Extensions;
using NetTemplate.Shared.WebApi.Logging.Extensions;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace NetTemplate.Shared.WebApi.Common.Utils
{
    public static class ProgramHelper
    {
        const string SupportedLoggers = nameof(SupportedLoggers);
        const string FileLoggerSectionName = nameof(Serilog) + ":" + SupportedLoggers + ":FileLogger";
        const string ConsoleLoggerSectionName = nameof(Serilog) + ":" + SupportedLoggers + ":ConsoleLogger";

        public static IHostBuilder CreateHostBuilder<TStartup>(string[] args) where TStartup : class =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog((context, services, configuration) =>
                {
                    configuration
                        .ReadFrom.Configuration(context.Configuration, sectionName: nameof(Serilog))
                        .ReadFrom.Services(services)
                        .WriteTo.Logger(fileConfig =>
                            fileConfig.ReadFrom.Configuration(
                                context.Configuration,
                                sectionName: FileLoggerSectionName)
                            .Filter.WithFileLoggerFilter());

                    if (!context.HostingEnvironment.IsProduction())
                    {
                        configuration = configuration.WriteTo.Logger(consoleConfig =>
                            consoleConfig.ReadFrom.Configuration(
                                context.Configuration,
                                sectionName: ConsoleLoggerSectionName));
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TStartup>();
                });

        public static Logger CreateHostLogger() => new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.WithUtcTimestamp()
                .WriteTo.HostLevelLog()
                .CreateLogger();
    }
}
