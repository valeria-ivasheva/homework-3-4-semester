using MyNUnit.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrongTests
{
    public class WrongTest
    {
        [Test]
        public void Test()
        {
            System.Threading.Thread.Sleep(50);
            throw new Exception();
        }
    }
}
