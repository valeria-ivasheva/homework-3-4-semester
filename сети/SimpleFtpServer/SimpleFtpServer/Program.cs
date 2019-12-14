using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace SimpleServer
{
  class Program
  {
    static void Main(string[] args)
    {
      var server = new Server(22234);
      Task.Run(async () => await server.Start());
      Console.ReadKey();
      server.Stop();
    }
  }
}
