using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFtpClient
{
    class MyFile
    {
        public string Name { get; private set; }
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
