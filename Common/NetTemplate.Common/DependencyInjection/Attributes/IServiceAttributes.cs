namespace NetTemplate.Common.DependencyInjection.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ServiceAttribute : Attribute
    {
    }

    public class TransientServiceAttribute : ServiceAttribute
    {
    }

    public class ScopedServiceAttribute : ServiceAttribute
    {
    }

    public class SingletonServiceAttribute : ServiceAttribute
    {
    }

    public class SelfTransientServiceAttribute : ServiceAttribute
    {
    }

    public class SelfScopedServiceAttribute : ServiceAttribute
    {
    }

    public class SelfSingletonServiceAttribute : ServiceAttribute
    {
    }
}
