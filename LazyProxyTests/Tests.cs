using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using LazyProxy;
using NUnit.Framework;

namespace LazyProxyTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public async Task Test1Async()
        {
            var req = new Request();
            var factory = new TestProxy();
            var lazyProxy = new LazyProxy<Request, Response>(factory, TimeSpan.FromMinutes(2));

            var tasks = new Task[1000];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = lazyProxy.ProcessOnceAsync("key", req);
            }

            await Task.WhenAll(tasks);

            // check that all responses have the same value of 1
            Assert.That(tasks.All(t => (t as Task<Response>).Result.Number == 1));

            // check that request has been processed only once
            Assert.That(req.Number == 1);
        }
    }


    class TestProxy : IProxy<Request, Response>
    {
        public async Task<Response> ProcessAsync(Request request)
        {
            var resp = new Response
            {
                Number = ++request.Number
            };
            await Task.Delay(TimeSpan.FromMilliseconds(1000)).ConfigureAwait(false);
            return resp;
        }
    }

    class Request
    {
        public int Number { get; set; }
    }

    public class Response
    {
        public int Number { get; set; }
    }
}
