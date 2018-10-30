using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFtpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server(888);
            server.Start();
        }
    }
}
