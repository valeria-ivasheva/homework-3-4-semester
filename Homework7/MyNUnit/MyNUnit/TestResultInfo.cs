using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNUnit
{
    /// <summary>
    /// Результаты теста
    /// </summary>
    public class TestResultInfo : IComparable<TestResultInfo>
    {
        /// <summary>
        /// Имя теста
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Время за которое проходит тест
        /// </summary>
        public long RunTime { get; }

        /// <summary>
        /// Результаты теста
        /// </summary>
        public ResultType Result { get; }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string Message { get; }

        public TestResultInfo(string name, long runTime)
        {
            Name = name;
            RunTime = runTime;
            Result = ResultType.OK;
        }

        public TestResultInfo(string name, bool isIgnored, string message)
        {
            Name = name;
            Result = isIgnored ? ResultType.IGNORED : ResultType.FAILED;
            Message = message;
        }

        /// <summary>
        /// Типы результата
        /// </summary>
        public enum ResultType
        {
            OK, FAILED, IGNORED
        }

        /// <summary>
        /// Распечатать результаты
        /// </summary>
        public void Write()
        {
            if (Result == ResultType.OK)
            {
                Console.WriteLine($"{Name} {Result} {RunTime}");
            }
            else
            {
                Console.WriteLine($"{Name} {Result} {Message}");
            }
        }

        /// <summary>
        /// Сравнение TestResultInfo
        /// </summary>
        /// <param name="other"> Объект, с которым происходит сравнение</param>
        /// <returns></returns>
        public int CompareTo(TestResultInfo other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }
}
