using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNUnit
{
    class Program
    {
        static void Main(string[] args)
        {
            //if (args.Length != 1)
            //{
            //    Console.WriteLine("Путь не найден");
            //    return;
            //}
            //TestingSystem.Run(args[0]);
            string path = @"C:\Users\ACER\ДомашкаПоПроге\homework-3-semester\Homework7\MyNUnit\TestProjects\TestWithException\bin";
            TestingSystem.Run(path);
            Console.ReadKey();
        }
    }
}
