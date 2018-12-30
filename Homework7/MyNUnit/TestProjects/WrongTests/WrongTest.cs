using MyNUnit.Attributes;
using System;

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
