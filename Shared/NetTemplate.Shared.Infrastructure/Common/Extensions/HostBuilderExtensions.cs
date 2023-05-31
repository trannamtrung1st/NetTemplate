using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetTemplate.Shared.Infrastructure.Logging.Extensions;
using Serilog;

namespace NetTemplate.Shared.Infrastructure.Common.Extensions
{
    public static class HostBuilderExtensions
    {
        const string SupportedLoggers = nameof(SupportedLoggers);
        const string FileLoggerSectionName = nameof(Serilog) + ":" + SupportedLoggers + ":FileLogger";
        const string ConsoleLoggerSectionName = nameof(Serilog) + ":" + SupportedLoggers + ":ConsoleLogger";

        public static IHostBuilder UseDefault(this IHostBuilder hostBuilder)
        {
            return hostBuilder
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
                });
        }
    }
}
