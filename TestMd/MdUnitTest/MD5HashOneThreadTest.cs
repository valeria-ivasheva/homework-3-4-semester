using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMd;

namespace MdUnitTest
{
    [TestClass]
    public class MD5HashOneThreadTest
    {
        [TestMethod]
        public void SimpleEmptyDirectory()
        {
            string path = @"C:\Games";
            var temp = new MD5HashOneThread(path);
            var result = temp.GetMdHash();
            Assert.AreEqual(result, "251bd8143891238ecedc306508e29017");
        }

        [TestMethod]
        public void SimpleFileTest()
        {
            string path = @"D:\summerSchool\Test.txt";
            var temp = new MD5HashOneThread(path);
            var tempMd = MD5.Create();
            string strRes = GetMd5Hash(tempMd, "ololo\npololo\n1234567890");
            var result = temp.GetMdHash();
            Assert.AreEqual(result, strRes);
        }

        private string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
