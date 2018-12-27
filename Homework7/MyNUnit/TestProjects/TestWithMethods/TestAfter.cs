using MyNUnit.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWithMethods
{
    public class TestAfter
    {
        private int count = 1;

        [AfterTest]
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
