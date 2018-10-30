using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFtpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client("localhost", 888);

            var result = client.List("C:");
            Console.WriteLine("Hello World!");
        }
    }
}
