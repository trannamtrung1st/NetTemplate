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
            return logger.ForContext(LogProperties.CallerMemberName, memberName)
                .ForContext(LogProperties.CallerFilePath, fileName)
                .ForContext(LogProperties.CallerLineNumber, lineNumber);
        }


        internal static class LogProperties
        {
            public const string CallerMemberName = nameof(CallerMemberName);
            public const string CallerFilePath = nameof(CallerFilePath);
            public const string CallerLineNumber = nameof(CallerLineNumber);
        }
    }
}
