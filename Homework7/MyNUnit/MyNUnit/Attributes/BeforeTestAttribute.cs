using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNUnit.Attributes
{
    /// <summary>
    /// Определяет атрибут, указывающий на запуск метода перед каждым тестом
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class BeforeTestAttribute : Attribute
    {
    }
}
