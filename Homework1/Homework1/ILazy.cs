using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1
{
    /// <summary>
    /// Интерфейс, представляющий ленивое вычисление
    /// </summary>
    /// <typeparam name="T"> Тип результата</typeparam>
    public interface ILazy<T>
    {
        /// <summary>
        /// Вызывает вычисление и возвращает результат 
        /// </summary>
        /// <returns>  Повторные вызовы Get() возвращают тот же объект, что и первый вызов</returns>
        T Get();
    }
}
