namespace NetTemplate.Common.Objects.Implementations
{

    public class ImmutableObject
    {
        public static ImmutableObject<TObj> Create<TObj>(TObj obj) where TObj : ICloneable
            => new ImmutableObject<TObj>(obj);
    }

    public class ImmutableObject<T> : ImmutableObject where T : ICloneable
    {
        private readonly T _obj;

        public ImmutableObject(T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            _obj = (T)obj.Clone();
        }

        public T GetClone() => (T)_obj.Clone();
    }
}
