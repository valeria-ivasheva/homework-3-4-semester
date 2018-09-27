using System;

namespace Homework1
{
    /// <summary>
    /// Класс Lazy, работающий в однопоточном режиме
    /// </summary>
    /// <typeparam name="T"> Тип функции</typeparam>
    public class Lazy<T> : ILazy<T>
    {
        private T result;
        private Func<T> func;
        private bool isResultCalculated = false;

        public Lazy(Func<T> supplier)
        {
            func = supplier;
        }

        /// <summary>
        /// Первый вызов Get() вызывает вычисление и возвращает результат
        /// </summary>
        /// <returns> Первый вызов возвращает результат, последующие первый</returns>
        public T Get()
        {
            if (!isResultCalculated)
            {
                result = func();
                func = null;
                isResultCalculated = true;
            }
            return result;
        }
    }
}
