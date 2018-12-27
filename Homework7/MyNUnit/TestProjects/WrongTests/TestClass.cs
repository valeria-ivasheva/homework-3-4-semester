using MyNUnit.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrongTests
{
    public class TestClass
    {
        [BeforeTest]
        public void Before()
        {
            throw new Exception();
        }

        [Test(Expected =typeof(Exception))]
        public void Test()
        {
            return;
        }
    }
}
