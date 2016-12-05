using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace LazyProxy
{
    internal class AsyncLazy<T>
    {
        private readonly Lazy<Task<T>> _lazy;

        public AsyncLazy(Func<T> factory)
        {
            _lazy = new Lazy<Task<T>>(() => Task.Run(factory));
        }

        public AsyncLazy(Func<Task<T>> factory)
        {
            _lazy = new Lazy<Task<T>>(factory);
        }


        public TaskAwaiter<T> GetAwaiter()
        {
            return _lazy.Value.GetAwaiter();
        }
    }
}
