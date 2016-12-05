using System.Threading.Tasks;

namespace LazyProxy
{
    public interface IProxy<in TRequest, TResponse>
    {
        Task<TResponse> ProcessAsync(TRequest request);
    }
}
