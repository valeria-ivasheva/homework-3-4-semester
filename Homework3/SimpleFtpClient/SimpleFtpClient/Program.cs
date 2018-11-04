using System;
using SimpleFtpServer;
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

            var clientOne = new Client("localhost", 22234);
            var client = new Client("localhost", 22234);
            var server = new Server(22234);
            server.Start();
            var pathToList = Console.ReadLine();
            var resultList = clientOne.List(pathToList);
            foreach (var temp in resultList)
            {
                Console.WriteLine($"{temp.Name} and {temp.IsDir}");
            }
            var savePath = Console.ReadLine();
            var pathToGet = Console.ReadLine();
            var result = client.Get(pathToGet, savePath);
            Console.WriteLine(result);
            server.Stop();
            Console.ReadKey();
        }
    }
}
