namespace LazyProxy
{
    public interface IProxy<in TRequest, TResponse>
    {
        TResponse Get(TRequest request);
    }
}
