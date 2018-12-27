using MyNUnit.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWithMethods
{
    public class TestAfterClass
    {
        private int count = 1;

        [AfterClass]
        public void AfterTest()
        {
            count = count + 2;
            throw new Exception();
        }

        [Test]
        public int Test()
        {
            return 1 / count;
        }

        [Test]
        public void Test1()
        {
            return;
        }
    }
}
