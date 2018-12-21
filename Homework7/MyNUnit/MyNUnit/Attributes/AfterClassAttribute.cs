using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNUnit.Attributes
{
    /// <summary>
    /// Определяет атрибут, указывающий на запуск метода после всех тестов
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AfterClassAttribute : Attribute
    {
    }
}
