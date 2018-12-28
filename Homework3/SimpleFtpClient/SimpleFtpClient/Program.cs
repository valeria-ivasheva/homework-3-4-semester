using SimpleFtpServer;
using System;

namespace SimpleFtpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client("localhost", 22234);
            var temp = client.List(@"C:\Users\ACER\ДомашкаПоПроге\homework-3-semester\Homework3\SimpleFtpClient");
            foreach( var pop in temp)
            {
                Console.WriteLine(pop.Name);
            }
            Console.Read();
        }
    }
}
