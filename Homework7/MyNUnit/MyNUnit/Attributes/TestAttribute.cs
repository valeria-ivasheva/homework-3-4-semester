using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNUnit.Attributes
{
    /// <summary>
    /// Определяет атрибут, указывающий на запуск теста
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TestAttribute : Attribute
    {
        /// <summary>
        ///  Отмены запуска и указание причины
        /// </summary>
        public string Ignore { get; set; }

        /// <summary>
        /// Ожидающееся исключение
        /// </summary>
        public Type Expected { get; set; }

        public TestAttribute()
        {
        }

        public TestAttribute(Type typeExpected)
        {
            Expected = typeExpected;
        }

        public TestAttribute(string strIgnore)
        {
            Ignore = strIgnore;
        }

        public TestAttribute(Type typeExpected, string strIgnore)
        {
            Expected = typeExpected;
            Ignore = strIgnore;
        }
    }
}
