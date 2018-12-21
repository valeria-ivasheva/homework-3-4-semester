using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNUnit
{
    public class InfoMethod
    {
        public Exception Exception { get; private set; }
        public string Name { get; private set; }
        public bool Result { get; private set; }

        public InfoMethod(string name, Exception exception)
        {
            Name = name;
            Exception = exception;
            Result = false;
        }

        public InfoMethod(string name)
        {
            Name = name;
            Result = true;
            Exception = null;
        }
    }
}
