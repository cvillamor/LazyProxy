using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace LazyProxy
{
    public class LazyProxy<TRequest, TResponse>
    {
        private readonly IProxy<TRequest, TResponse> _proxy;
        private readonly MemoryCache _cache;
        private readonly TimeSpan _cacheExpirationTime;

        public LazyProxy(IProxy<TRequest, TResponse> proxy, TimeSpan cacheExpirationTime)
        {
            _cache = new MemoryCache("LazyProzy.RequestCache");
            _cacheExpirationTime = cacheExpirationTime;
            _proxy = proxy;
        }

        public void Delete(string key)
        {
            _cache.Remove(key);
        }

        public async Task<TResponse> ProcessOnceAsync(string key, TRequest request)
        {
            var val = _cache.AddOrGetExisting(key, new AsyncLazy<TResponse>(() => _proxy.ProcessAsync(request)), DateTimeOffset.UtcNow.Add(_cacheExpirationTime)) as AsyncLazy<TResponse>;

            // the first thread to insert into the cache will get a null value as return
            if (val == null)
            {
                val = _cache.Get(key) as AsyncLazy<TResponse>;
            }

            return await val;
        }

        
    }
}
