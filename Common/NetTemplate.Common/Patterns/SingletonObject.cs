namespace NetTemplate.Common.Patterns
{
    public class SingletonObject
    {
        private static Lazy<SingletonObject> _instance = new Lazy<SingletonObject>();

        private SingletonObject() { }

        public static SingletonObject Instance => _instance.Value;
    }
}
