using System;

namespace MyThreadPool
{
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