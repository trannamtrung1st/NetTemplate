using NetTemplate.Common.Reflection.Extensions;
using System.Reflection;

namespace NetTemplate.Common.Logging.Extensions
{
    public static class MethodInfoExtensions
    {
        public static string GetDescription(this MethodInfo methodInfo, object[] arguments)
        {
            var paramDesc = methodInfo.GetParameters()
                .Select((param, idx) =>
                {
                    var paramType = param.ParameterType.GetGenericTypeName();
                    var argsType = arguments[idx]?.GetGenericTypeName();
                    return paramType == argsType
                        ? $"{paramType} {param.Name}"
                        : $"{paramType}:{argsType} {param.Name}";
                });
            var paramDescStr = string.Join(", ", paramDesc);
            return $"{methodInfo.ReturnType.GetGenericTypeName()} {methodInfo.Name}({paramDescStr})";
        }
    }
}
