using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNUnit.Attributes;

namespace TestIgnore
{
    public class Ignore
    {
        [Test]
        public void TempTest()
        {
            var point = 21 + 2;
            point.GetHashCode();
        }
            
        [Test(Ignore="Просто так)")]
        public void IgnoreTest()
        {
            Console.WriteLine("Это не должно было случиться");
        }
    }
}
