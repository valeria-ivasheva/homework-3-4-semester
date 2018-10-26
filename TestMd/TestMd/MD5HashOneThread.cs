using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TestMd
{
    public class MD5HashOneThread
    {
        private string path;

        public MD5HashOneThread(string path)
        {
            this.path = path;
        }

        public string GetMdHash()
        {
            var result = "";
            if (File.Exists(path))
            {
                result = ProcessFile(path);
            }
            else
            {
                if (Directory.Exists(path))
                {
                    result = ProcessDirectory(path);
                }
                else
                {
                    Console.WriteLine("{0} is not a valid file or directory.", path);
                    throw new Exception ($"{path} is not a valid file or directory.");
                }
            }
            return result;
        }

        private  string GetContent()
        {
            FileInfo file = new FileInfo(path);
            string content = "";
            using (StreamReader sr = file.OpenText())
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    content = content + s + "/n";
                }
            }
            return content;
        }

        private string ProcessFile(string path)
        {
            var content = GetContent();
            var md5Hash = MD5.Create();
            string result = GetMd5Hash(md5Hash, content);
            return result;
        }

        private string ProcessDirectory(string path)
        {
            DirectoryInfo dInfo = new DirectoryInfo(path);
            string input = dInfo.Name;
            var directories = dInfo.GetDirectories().OrderBy(d => d.Name).ToList();
            var files = dInfo.GetFiles().OrderBy(f => f.Name).ToList();
            foreach (var item in directories)
            {
                input += ProcessDirectory(item.FullName);
            }
            foreach (var item in files)
            {
                input += ProcessFile(item.FullName);
            }
            var md5Hash = MD5.Create();
            string result = GetMd5Hash(md5Hash, input);
            return result;
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
