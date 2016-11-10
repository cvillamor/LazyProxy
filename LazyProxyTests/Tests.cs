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
        public void Test1()
        {
            var req = new Req();
            var factory = new TestFactory();
            var lazyProxy = new LazyProxy<Req,Resp>(factory, TimeSpan.FromMinutes(2));

            var tasks = new Task[1000];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() => lazyProxy.ProcessOnce("key", req));
            }

            Task.WaitAll(tasks);
            Assert.That(tasks.All(t => (t as Task<Resp>).Result.Number == 1));
            Assert.That(req.Number == 1);
        }
    }

    class TestFactory : IFactory<Req, Resp>
    {
        public Resp Get(Req req)
        {
            var resp = new Resp
            {
                Number = ++req.Number
            };
            Thread.Sleep(TimeSpan.FromMilliseconds(1000));
            return resp;
        }
    }

    class Req
    {
        public int Number { get; set; }
    }

    public class Resp
    {
        public int Number { get; set; }
    }
}
