using System;

namespace SimpleFtpClient
{
    /// <summary>
    /// Информация о файле
    /// </summary>
    public class MyFile
    {
        /// <summary>
        /// Имя файла
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Это директория
        /// </summary>
        public bool IsDir { get; private set; }

        public MyFile(string name, bool isDir)
        {
            Name = name;
            IsDir = isDir;
        }

        public MyFile(string str)
        {
            var strTemp = str.Split(' ');
            Name = strTemp[0];
            IsDir = Convert.ToBoolean(strTemp[1]);
        }
    }
}
