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
        private AutoResetEvent threadReset = new AutoResetEvent(false);
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
        /// <param name="func"> Задача, для добавления</param>
        public IMyTask<T> AddTask<T>(Func<T> func)
        {
            var task = new MyTask<T>(this, func);
            if (cts.IsCancellationRequested)
            {
                throw new ThreadPoolClosedException();
            }
            taskQueue.Enqueue(task.Get);
            threadReset.Set();
            return task;
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

        private class MyTask<T> : IMyTask<T>
        {
            private readonly Func<T> func;
            private T tResult;
            private readonly MyThreadPool threadPool;
            private ManualResetEvent resultReset = new ManualResetEvent(false);
            private ConcurrentQueue<Action> funcsContinue = new ConcurrentQueue<Action>();
            private static object locker = new Object();
            private Exception exception = null;

            /// <summary>
            /// Задача — вычисление некоторого значения, описывается в виде Func<TResult>
            /// </summary>
            /// <param name="myThreadPool"> Пул потоков, в котором проходит вычисление</param>
            /// <param name="func"> Сама задача</param>
            public MyTask(MyThreadPool myThreadPool, Func<T> func)
            {
                this.func = func;
                threadPool = myThreadPool;
                IsCompleted = false;
            }

            /// <summary>
            /// Возвращает true, если задача выполнена
            /// </summary>
            public bool IsCompleted { get; private set; }

            /// <summary>
            /// Результата задачи
            /// </summary>
            public T Result
            {
                get
                {
                    resultReset.WaitOne();
                    if (exception == null)
                    {
                        return tResult;
                    }
                    else
                    {
                        throw new AggregateException(exception.Message);
                    }
                }
            }

            /// <summary>
            /// Новая задача, работающая на основе результата старой
            /// </summary>
            /// <typeparam name="TNewResult"> Тип результата новой задачи Y</typeparam>
            /// <param name="func"> Объект, который может быть применен к результату данной задачи X</param>
            /// <returns> Новая задача Y, принятая к исполнению</returns>
            public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<T, TNewResult> func)
            {
                IMyTask<TNewResult> newTask = new MyTask<TNewResult>(threadPool, () => func(Result));
                lock (locker)
                {
                    if (IsCompleted)
                    {
                        return threadPool.AddTask(() => func(Result));
                    }
                    else
                    {
                        funcsContinue.Enqueue(() => { threadPool.AddTask(() => func(Result)); });
                    }
                }
                return newTask;
            }

            /// <summary>
            /// Запуск подсчета результата
            /// </summary>
            public void Get()
            {
                try
                {
                    tResult = func();
                }
                catch (Exception e)
                {
                    exception = e;
                }
                IsCompleted = true;
                foreach (var temp in funcsContinue)
                {
                    funcsContinue.TryDequeue(out Action task);
                    task();
                }
                resultReset.Set();
            }
        }
    }
}
