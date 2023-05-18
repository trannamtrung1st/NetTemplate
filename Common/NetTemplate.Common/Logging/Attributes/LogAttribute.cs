namespace NetTemplate.Common.Logging.Attributes
{

    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method,
        AllowMultiple = false, Inherited = true)]
    public class LogAttribute : Attribute
    {
        public bool PublicOnly { get; set; }
        public bool Disabled { get; set; }
    }
}
