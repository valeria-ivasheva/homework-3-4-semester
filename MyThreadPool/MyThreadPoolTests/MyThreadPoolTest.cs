using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyThreadPool;
using System;

namespace MyThreadPoolTests
{
    [TestClass]
    public class MyThreadPoolTest
    {
        private MyThreadPool.MyThreadPool myThreadPool;

        [TestInitialize]
        public void TestInitialize()
        {
            myThreadPool = new MyThreadPool.MyThreadPool(10);
        }

        [TestMethod]
        public void ContinueTest()
        {
            var task = myThreadPool.AddTask(() => 22);
            var newTask = task.ContinueWith(j => j * 5);
            Assert.AreEqual(task.Result, 22);
            Assert.AreEqual(newTask.Result, 110);
        }

        [TestMethod]
        public void SimpleThreadTest()
        {
            var tasks = new IMyTask<string>[100];
            for (int i = 0; i < 100; i++)
            {
                int temp = i;
                tasks[i] = myThreadPool.AddTask(() => $"It is {temp++}");
            }
            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual(tasks[i].Result, $"It is {i++}");
            }
        }

        [TestMethod]
        public void TestOfThread()
        {
            var task1 = myThreadPool.AddTask(() => 36);
            var task2 = myThreadPool.AddTask(() => "AAAAA");
            var task3 = task1.ContinueWith(j => System.Math.Sqrt(j));
            var task4 = myThreadPool.AddTask(() => TempFunc(task2.Result, task3.Result));
            var strResult = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
            Assert.AreEqual(strResult, task4.Result);
        }

        private string TempFunc(string str, double count)
        {
            var result = "";
            for (int i = 0; i < (count); i++)
            {
                result += str;
            }
            return result;
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException), AllowDerivedTypes = true)]
        public void TestWithException()
        {
            var tasks = new IMyTask<int>[100];
            for (int i = 0; i < 100; i++)
            {
                int tempI = i;
                tasks[i] = myThreadPool.AddTask(() => tempI / (tempI - tempI));
            }
            var temp = tasks[0].Result;
        }

        [TestMethod]
        public void CountOfThreadTest()
        {
            var tasks = new IMyTask<string>[100];
            for (int i = 0; i < 100; i++)
            {
                int temp = i;
                tasks[i] = myThreadPool.AddTask(() => $"It is {temp++}");
            }
            Assert.AreEqual(10, myThreadPool.CountOfAliveThreads());
        }

        [TestMethod]
        [ExpectedException(typeof(ThreadPoolClosedException))]
        public void ShutdowntTest()
        {
            var tasks = new IMyTask<int>[100];
            for (int i = 0; i < 100; i++)
            {
                tasks[i] = myThreadPool.AddTask(() => 100);
            }
            myThreadPool.Shutdown();
            var tempTask = myThreadPool.AddTask(() => "Ololo");
            Assert.IsNull(tempTask);
            for (int i = 0; i < 100; i++)
            {
                Assert.IsTrue(tasks[i].IsCompleted);
                Assert.AreEqual(100, tasks[i].Result);
            }
        }
    }
}

