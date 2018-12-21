using System;

namespace MyThreadPool
{
    /// <summary>
    /// Исключение, пул потоков закрыт
    /// </summary>
    public class ThreadPoolClosedException : Exception
    {
        public ThreadPoolClosedException()
        {
        }

        public ThreadPoolClosedException(string message) : base(message)
        {
        }
    }
}