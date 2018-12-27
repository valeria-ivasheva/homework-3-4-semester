using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNUnit.Attributes;

namespace TestWithMethods
{
    public class TestWithBeforeClass
    {
        private int count = 0;

        [BeforeClass]
        public void BeforeClass()
        {
            count = count + 2;
        }

        [Test]
        public void Test1()
        {
            return;
        }
    }
}
