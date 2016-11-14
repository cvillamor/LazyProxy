namespace LazyProxy
{
    public interface IProxy<in TRequest, out TResponse>
    {
        TResponse Process(TRequest request);
    }
}
