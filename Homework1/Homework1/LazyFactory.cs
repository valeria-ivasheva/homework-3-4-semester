using System;

namespace Homework1
{
    /// <summary>
    /// Класс, создающий объект Lazy 
    /// </summary>
    public class LazyFactory
    {
        /// <summary>
        /// Создает класс, работающий в однопоточном режиме
        /// </summary>
        /// <typeparam name="T"> Тип функций</typeparam>
        /// <param name="supplier"> Вычисление, лежащее в основе объекта</param>
        /// <returns> Класс Lazy, для однопоточного режима</returns>
        public static ILazy<T> CreateOneThreadLazy<T>(Func<T> supplier)
        {
            return new Lazy<T>(supplier);
        }

        /// <summary>
        /// Создает класс, работающий в многопоточном режиме
        /// </summary>
        /// <typeparam name="T"> Тип функций</typeparam>
        /// <param name="supplier"> Вычисление, лежащее в основе объекта</param>
        /// <returns> Класс MultiThreadLazy, для многопоточного режима</returns>
        public static ILazy<T> CreateMultiThreadLazy<T>(Func<T> supplier)
        {
            return new MultiThreadLazy<T>(supplier);
        }
    }
}

