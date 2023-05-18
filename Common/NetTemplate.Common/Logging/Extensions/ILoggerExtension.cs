using Serilog;
using System.Runtime.CompilerServices;

namespace NetTemplate.Common.Logging.Extensions
{
    public static class ILoggerExtensions
    {
        public static ILogger CallerContext(this ILogger logger,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string fileName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            return logger.ForContext(LoggingConstants.LogProperties.CallerMemberName, memberName)
                .ForContext(LoggingConstants.LogProperties.CallerFilePath, fileName)
                .ForContext(LoggingConstants.LogProperties.CallerLineNumber, lineNumber);
        }
    }
}
