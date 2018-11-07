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
            var pathToList = @"D:\Zona Downloads"; ///Console.ReadLine();
            var resultList = clientOne.List(pathToList);
            foreach (var temp in resultList)
            {
                Console.WriteLine($"{temp.Name} and {temp.IsDir}");
            }
            var savePath = @"D:\Zona Downloads\test.txt";///Console.ReadLine();
            var pathToGet = @"D:\text.txt";///Console.ReadLine();
            var result = client.Get(pathToGet, savePath);
            Console.WriteLine(result);
            server.Stop();
            Console.ReadKey();
        }
    }
}
