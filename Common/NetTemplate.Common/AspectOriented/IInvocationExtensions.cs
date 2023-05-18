using Castle.DynamicProxy;

namespace NetTemplate.Common.AspectOriented
{
    public static class IInvocationExtensions
    {
        public static T ProceedAsyncMethod<T>(this IInvocation invocation)
        {
            invocation.Proceed();
            var task = invocation.ReturnValue as Task<T>;
            return task != null ? task.Result : default;
        }

        public static void ProceedAsyncMethod(this IInvocation invocation)
        {
            invocation.Proceed();
            var task = invocation.ReturnValue as Task;
            if (task != null)
            {
                task.Wait();
            }
        }
    }
}
