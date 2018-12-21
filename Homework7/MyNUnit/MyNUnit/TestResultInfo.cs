using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNUnit
{
    public class TestResultInfo
    {
        public string Name { get; }
        public long RunTime { get; }
        public ResultType Result { get; }
        public string Message { get; }

        public TestResultInfo(string name, long runTime)
        {
            Name = name;
            RunTime = runTime;
            Result = ResultType.OK;
        }

        public TestResultInfo(string name, bool isIgnored, string message)
        {
            Name = name;
            Result = isIgnored ? ResultType.IGNORED : ResultType.FAILED;
            Message = message;
        }

        public enum ResultType
        {
            OK, FAILED, IGNORED
        }
    }
}
