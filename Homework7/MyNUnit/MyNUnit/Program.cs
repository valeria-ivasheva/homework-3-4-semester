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
            string path = @"C:\Users\ACER\ДомашкаПоПроге\homework-3-semester\Homework7\MyNUnit\TestProject\bin\Debug";
            TestingSystem.Run(path);
            Console.ReadKey();
            //TestingSystem.Run(args[0]);
        }
    }
}
