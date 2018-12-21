using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFtpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server(22234);
            Task.Run(async () => await server.Start());
            Console.Read();
            server.Stop();
        }
    }
}
