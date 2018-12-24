using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNUnit
{
    /// <summary>
    /// Результат завершения метода
    /// </summary>
    public class InfoMethod
    {
        /// <summary>
        /// Ошибка, возникшая при работе метода
        /// </summary>
        public Type Exception { get; private set; }

        /// <summary>
        /// Имя метода
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Прошел метод без ошибок
        /// </summary>
        public bool Result { get; private set; }

        public InfoMethod(string name, Type exception)
        {
            Name = name;
            Exception = exception;
            Result = false;
        }

        public InfoMethod(string name)
        {
            Name = name;
            Result = true;
            Exception = null;
        }
    }
}
