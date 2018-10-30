using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyThreadPool
{
    /// <summary>
    /// Задача— вычисление некоторого значения
    /// </summary>
    /// <typeparam name="T"> Тип результата задачи</typeparam>
    public class MyTask<T> : IMyTask<T>
    {
        private readonly Func<T> func;
        private T tResult;
        private readonly MyThreadPool threadPool;
        private ManualResetEvent resultReset = new ManualResetEvent(false);
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
            threadPool.AddTask(this);
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
        public MyTask<TNewResult> ContinueWith<TNewResult>(Func<T, TNewResult> func)
        {
            TNewResult newFunc() => func(Result);
            MyTask<TNewResult> newTask = new MyTask<TNewResult>(threadPool, newFunc);
            threadPool.AddTask(newTask);
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
            resultReset.Set();
        }
    }
}