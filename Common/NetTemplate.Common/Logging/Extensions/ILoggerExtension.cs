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
            return logger.ForContext(Constants.LogProperties.CallerMemberName, memberName)
                .ForContext(Constants.LogProperties.CallerFilePath, fileName)
                .ForContext(Constants.LogProperties.CallerLineNumber, lineNumber);
        }
    }
}
