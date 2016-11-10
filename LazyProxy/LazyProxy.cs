using System;
using System.Runtime.Caching;

namespace LazyProxy
{
    public class LazyProxy<TRequest, TResponse>
    {
        private readonly IFactory<TRequest, TResponse> _factory;
        private readonly MemoryCache _cache;
        private readonly TimeSpan _cacheExpirationTime;

        public LazyProxy(IFactory<TRequest, TResponse> factory, TimeSpan cacheExpirationTime)
        {
#if false
            if (timeout.Equals(TimeSpan.MaxValue) || timeout.Equals(TimeSpan.MinValue))
            {
                throw new ArgumentException("Must provide a valid timeout");
            } 
#endif
            _cache = new MemoryCache("LazyProzy.RequestCache");
            _cacheExpirationTime = cacheExpirationTime;
            _factory = factory;
        }

        public void Delete(string key)
        {
            _cache.Remove(key);
        }

        public TResponse ProcessOnce(string key, TRequest request)
        {
            var val = _cache.AddOrGetExisting(key, new Lazy<TResponse>(() => _factory.Get(request)), DateTimeOffset.UtcNow.Add(_cacheExpirationTime)) as Lazy<TResponse>;

            // the first thread to insert into the cache will get a null value as return
            if (val == null)
            {
                val = _cache.Get(key) as Lazy<TResponse>;
            }

            return val.Value;
        }

        
    }
}
