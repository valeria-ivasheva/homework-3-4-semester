using Homework1;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LazyTest
{
    [TestClass]
    public class LazyTest
    {
        [TestMethod]
        public void SimpleOneThreadTest()
        {
            var lazy = LazyFactory.CreateOneThreadLazy<int>(() => { return 21;});
            Assert.AreEqual(21, lazy.Get());
        }

        [TestMethod]
        public void CalculateOneThreadTest()
        {
            var x = 4;
            var lazy = LazyFactory.CreateOneThreadLazy<int>(() => x * x);
            Assert.AreEqual(16, lazy.Get());
        }

        [TestMethod]
        public void GetCalculateOnlyOnceTest()
        {
            var x = 4;
            var lazy = LazyFactory.CreateOneThreadLazy<int>(() => x * x);
            Assert.AreEqual(16, lazy.Get());
            lazy.Get();
            Assert.AreEqual(16, lazy.Get());
        }

        [TestMethod]
        public void StringSimpleTest()
        {
            var lazy = LazyFactory.CreateOneThreadLazy<string>(() => { return "LOLO"; });
            Assert.AreEqual("LOLO", lazy.Get());
        }

        [TestMethod]
        public void NullOneThreadTest()
        {
            var lazy = LazyFactory.CreateOneThreadLazy<object>(() => { return null; });
            Assert.IsNull(lazy.Get());
        }
    }
}
