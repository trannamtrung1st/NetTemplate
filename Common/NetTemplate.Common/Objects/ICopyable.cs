namespace NetTemplate.Common.Objects
{
    public interface ICopyable<T>
    {
        void CopyTo(T other);
    }
}
