using System.Threading;
using Homework1;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LazyTest
{
    [TestClass]
    public class MultiThreadLazyTest
    {
        [TestMethod]
        public void RaceTest()
        {
            var lazy = LazyFactory.CreateMultiThreadLazy<int>(() => { return 12; });
            var threads = new Thread[100];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() =>
                {
                    lazy.Get();
                    Assert.AreEqual(12, lazy.Get());
                });
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }

        [TestMethod]
        public void SimpleMultiThreadTest()
        {
            var lazy = LazyFactory.CreateMultiThreadLazy<int>(() => { return 21; });
            Assert.AreEqual(21, lazy.Get());
        }

        [TestMethod]
        public void CalculateMultiThreadTest()
        {
            var x = 4;
            var lazy = LazyFactory.CreateMultiThreadLazy<int>(() => x * x);
            Assert.AreEqual(16, lazy.Get());
        }

        [TestMethod]
        public void GetCalculateOnlyOnceTest()
        {
            var count = 0;
            var lazy = LazyFactory.CreateMultiThreadLazy<int>(() =>
            {
                count++;
                return 16;
            });
            Assert.AreEqual(16, lazy.Get());
            Assert.AreEqual(1, count);
            lazy.Get();
            Assert.AreEqual(16, lazy.Get());
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void NullMultiThreadTest()
        {
            var lazy = LazyFactory.CreateMultiThreadLazy<object>(() => { return null; });
            Assert.IsNull(lazy.Get());
        }
    }
}
