using System;

namespace MyNUnit
{
    class Program
    {
        /// <summary>
        /// Запускает тестировщик и распечатывает результаты тестов
        /// </summary>
        /// <param name="args"> Путь, в котором находятся тесты</param>
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Путь не найден");
                return;
            }
            TestingSystem.Run(args[0]);
            var result = TestingSystem.GetResultTestInfos();
            result.Sort();
            for(int i = 0; i < result.Count; i++)
            {
                result[i].Write();
            }
        }
    }
}
