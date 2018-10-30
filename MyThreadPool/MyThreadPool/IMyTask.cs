using System;

namespace MyThreadPool
{
    /// <summary>
    /// Интерфейс задач
    /// </summary>
    /// <typeparam name="T"> Тип результата задачи</typeparam>
    public interface IMyTask<T>
    {
        /// <summary>
        /// Возвращает true, если задача выполнена
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Возвращает результат выполнения задачи
        /// </summary>
        T Result { get; }

        /// <summary>
        /// Новая задача, работающая на основе результата старой
        /// </summary>
        /// <typeparam name="TNewResult"> Тип результата новой задачи Y</typeparam>
        /// <param name="func"> Объект, который может быть применен к результату данной задачи X</param>
        /// <returns> Новая задача Y, принятая к исполнению</returns>
        MyTask<TNewResult> ContinueWith<TNewResult>(Func<T, TNewResult> func);
    }
}
