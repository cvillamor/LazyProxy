namespace LazyProxy
{
    public interface IFactory<in TRequest, TResponse>
    {
        TResponse Get(TRequest request);
    }
}
