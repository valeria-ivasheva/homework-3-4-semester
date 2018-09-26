using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1
{
    /// <summary>
    /// Класс Lazy, работающий в многопоточном режиме
    /// </summary>
    /// <typeparam name="T"> Тип функции</typeparam>
    public class MultiThreadLazy<T> : ILazy<T>
    {
        private Func<T> func;
        private T result;
        private volatile bool isResultCalculated = false;
        private static object locker = new Object();

        public MultiThreadLazy(Func<T> supplier)
        {
            this.func = supplier;
        }

        /// <summary>
        /// Первый вызов Get() вызывает вычисление и возвращает результат
        /// </summary>
        /// <returns> Первый вызов возвращает результат, последующие первый </returns>
        public T Get()
        {
            if (!isResultCalculated)
            {
                lock (locker)
                {
                    if (!isResultCalculated)
                    {
                        result = func();
                        func = null;
                        isResultCalculated = true;
                    }
                }
            }
            return result;
        }
    }
}
