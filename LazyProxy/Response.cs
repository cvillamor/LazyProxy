namespace LazyProxy
{
    public class Response<T>
    {
        public bool Success { get; internal set; }
        public string Message { get; internal set; }
        public T Result { get; internal set; }
    }
}
