using System;
using System.Collections.Concurrent;
using System.Threading;

namespace MyThreadPool
{
    /// <summary>
    /// Пул задач с фиксированным числом потоков
    /// </summary>
    public class MyThreadPool
    {
        private readonly int countOfThread;
        private readonly Thread[] threads;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private AutoResetEvent threadReset = new AutoResetEvent(true);
        private ConcurrentQueue<Action> taskQueue = new ConcurrentQueue<Action>();

        /// <summary>
        /// Пул задач с фиксированным числом потоков
        /// </summary>
        /// <param name="countOfThread"> Количество потоков</param>
        public MyThreadPool(int countOfThread)
        {
            this.countOfThread = countOfThread;
            threads = new Thread[countOfThread];
            for (int i = 0; i < countOfThread; i++)
            {
                threads[i] = new Thread(() =>
                {
                    while (!cts.IsCancellationRequested)
                    {
                        if (taskQueue.TryDequeue(out Action task))
                        {
                            task();
                        }
                        else
                        {
                            threadReset.WaitOne();
                            if (cts.IsCancellationRequested)
                            {
                                threadReset.Set();
                            }
                        }
                    }
                });
                threads[i].IsBackground = true;
                threads[i].Start();
            }
        }

        /// <summary>
        /// Добавить задачу в пул
        /// </summary>
        /// <typeparam name="T"> Тип результата задачи</typeparam>
        /// <param name="task"> Задача, для добавления</param>
        public void AddTask<T>(MyTask<T> task)
        {
            if (cts.IsCancellationRequested)
            {
                return;
            }
            taskQueue.Enqueue(task.Get);
            threadReset.Set();
        }

        /// <summary>
        /// Количество потоков в пуле
        /// </summary>
        /// <returns></returns>
        public int CountOfAliveThreads()
        {
            int result = 0;
            for (int i = 0; i < countOfThread; i++)
            {
                if (threads[i].IsAlive)
                {
                    result++;
                }
            }
            return result;
        }

        /// <summary>
        /// Завершение потоков
        /// </summary>
        public void Shutdown()
        {
            cts.Cancel();
            threadReset.Set();
            taskQueue = null;
            for (int i = 0; i < countOfThread; i++)
            {
                threads[i].Join();
            }
        }
    }
}
