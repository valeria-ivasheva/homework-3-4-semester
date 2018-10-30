using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyThreadPool;
using System;

namespace MyThreadPoolTest
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
            var task = new MyTask<int>(myThreadPool, () => 22);
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
                tasks[i] = new MyTask<string>(myThreadPool, () => $"It is {temp++}");
            }
            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual(tasks[i].Result, $"It is {i++}");
            }
        }

        [TestMethod]
        public void TestOfThread()
        {
            var task1 = new MyTask<int>(myThreadPool, () => 36);
            var task2 = new MyTask<string>(myThreadPool, () => "AAAAA");
            var task3 = task1.ContinueWith(j => System.Math.Sqrt(j));
            var task4 = new MyTask<string>(myThreadPool, () => TempFunc(task2.Result, task3.Result));
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
                tasks[i] = new MyTask<int>(myThreadPool, () => tempI / (tempI - tempI));
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
                tasks[i] = new MyTask<string>(myThreadPool, () => $"It is {temp++}");
            }
            Assert.AreEqual(10, myThreadPool.CountOfAliveThreads());
        }

        [TestMethod]
        public void ShutdowntTest()
        {
            var tasks = new IMyTask<int>[100];
            for (int i = 0; i < 100; i++)
            {
                tasks[i] = new MyTask<int>(myThreadPool, () => 100);
            }
            myThreadPool.Shutdown();
            var tempTask = new MyTask<string>(myThreadPool, () => "Ololo");
            myThreadPool.AddTask(tempTask);
            Assert.IsFalse(tempTask.IsCompleted);
            for (int i = 0; i < 100; i++)
            {
                Assert.IsTrue(tasks[i].IsCompleted);
                Assert.AreEqual(100, tasks[i].Result);
            }
        }
    }
}
