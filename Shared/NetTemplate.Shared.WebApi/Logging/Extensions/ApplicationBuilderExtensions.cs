using NetTemplate.Common.Logging.Extensions;
using NetTemplate.Common.Logging.Options;
using Serilog;

namespace NetTemplate.Shared.WebApi.Logging.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app,
            IConfiguration configuration, string requestLoggingSection,
            out IDisposable customLogger)
        {
            customLogger = null;

            RequestLoggingOptions requestLoggingOptions = configuration
                .GetSection(requestLoggingSection)
                .Get<RequestLoggingOptions>();

            if (requestLoggingOptions != null)
            {
                if (!requestLoggingOptions.UseDefaultLogger)
                {
                    Serilog.Core.Logger requestLogger = configuration.ParseLogger(
                        requestLoggingSection, app.ApplicationServices);
                    customLogger = requestLogger;
                    app.UseSerilogRequestLogging(requestLoggingOptions, requestLogger);
                }
                else
                {
                    app.UseSerilogRequestLogging(requestLoggingOptions);
                }
            }

            return app;
        }

        private static IApplicationBuilder UseSerilogRequestLogging(this IApplicationBuilder app,
            RequestLoggingOptions frameworkOptions, Serilog.ILogger logger = null)
        {
            return app.UseSerilogRequestLogging(options =>
            {
                if (!frameworkOptions.UseDefaultLogger)
                    options.Logger = logger;

                // Customize the message template
                options.MessageTemplate = frameworkOptions.MessageTemplate;

                // Emit info-level events instead of the defaults
                if (frameworkOptions.StaticGetLevel != null)
                    options.GetLevel = (httpContext, elapsed, ex) => frameworkOptions.StaticGetLevel.Value;

                // Attach additional properties to the request completion event
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    if (frameworkOptions.IncludeHost)
                        diagnosticContext.Set(nameof(httpContext.Request.Host), httpContext.Request.Host);

                    foreach (var header in frameworkOptions.EnrichHeaders)
                        diagnosticContext.Set(header.Key, httpContext.Request.Headers[header.Value]);
                };
            });
        }
    }
}
