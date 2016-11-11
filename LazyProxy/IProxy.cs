namespace LazyProxy
{
    public interface IProxy<in TRequest, TResponse>
    {
        TResponse Process(TRequest request);
    }
}
