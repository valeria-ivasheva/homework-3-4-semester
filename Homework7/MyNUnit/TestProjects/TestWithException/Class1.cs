using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNUnit.Attributes;

namespace TestWithException
{
    public class Class1
    {
        [Test(Expected =typeof(DivideByZeroException))]
        public void Exception()
        {
            try
            {
                var div = 8 - 8;
                var count = 23 / div;
            }
            catch (Exception e)
            {
                throw new DivideByZeroException(e.Message);
            }
        }
    }
}
