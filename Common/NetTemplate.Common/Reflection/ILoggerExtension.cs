using System.Runtime.CompilerServices;

namespace NetTemplate.Common.Reflection
{
    public static class ObjectExtensions
    {
        public static (string MemberName, string FileName, int LineNumber) CallerContext(this object @object,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string fileName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            return (memberName, fileName, lineNumber);
        }
    }
}
