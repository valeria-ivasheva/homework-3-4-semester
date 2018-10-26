using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TestMd
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Games";
            var temp = new MD5HashOneThread(path);
            var result = temp.GetMdHash();
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
    

