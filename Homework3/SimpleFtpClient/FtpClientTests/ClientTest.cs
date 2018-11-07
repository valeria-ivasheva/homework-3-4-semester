using SimpleFtpServer;
using SimpleFtpClient;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FtpClientTests
{
    [TestClass]
    public class ClientTest
    {
        public Server server = new Server(22234);
        private Client client = new Client("localhost", 22234);
        private Client clientOne = new Client("localhost", 22234);
        private string path;

        [TestInitialize]
        public void Initialize()
        {
            server = new Server(22234);
            clientOne = new Client("localhost", 22234);
            client = new Client("localhost", 22234);
            path = Directory.GetCurrentDirectory();
            path = Directory.GetParent(path).FullName;
            path = Directory.GetParent(path).FullName;
            server.Start();
        }

        [TestMethod]
        public void ConnectedCorrectlyTest()
        {
            Assert.IsTrue(client.Connect());
        }

        [TestMethod]
        public void ListTest()
        {
            DirectoryInfo dInfo = new DirectoryInfo(path);
            var directories = dInfo.GetDirectories().OrderBy(d => d.Name).ToList();
            var files = dInfo.GetFiles().OrderBy(f => f.Name).ToList();
            var resultList = clientOne.List(path);
            Assert.AreEqual(directories.Count + files.Count, resultList.Count);
            var resultDir = resultList.Where(n => n.IsDir == true).ToList();
            var resultFiles = resultList.Where(n => n.IsDir == false).ToList();
            Assert.AreEqual(directories.Count, resultDir.Count);
            Assert.AreEqual(files.Count, resultFiles.Count);
            resultDir.OrderBy(d => d.Name);
            resultFiles.OrderBy(f => f.Name);
            for (int i = 0; i < directories.Count; i++)
            {
                Assert.AreEqual(directories[i].Name, resultDir[i].Name);
            }
            for (int i = 0; i < files.Count; i++)
            {
                Assert.AreEqual(files[i].Name, resultFiles[i].Name);
            }
        }

        [TestMethod]
        public void GetTest()
        {
            var lines = new string[3] { "Test", "22222, 322, 23", "Olololo" };
            var pathFile = path + "MyFile.txt";
            var pathSave = path + "MyTest.txt";
            using (StreamWriter file = new StreamWriter(pathFile))
            {
                foreach (string line in lines)
                    file.WriteLine(line);
            }
            var result = client.Get(pathFile, pathSave);
            Assert.IsTrue(result);
            using (StreamReader file = new StreamReader(pathSave))
            {
                for (int i = 0; i < 3; i++)
                {
                    Assert.AreEqual(lines[i], file.ReadLine());
                }
            }
            File.Delete(pathFile);
            File.Delete(pathSave);
        }

        [TestMethod]
        public void FileNonexistent()
        {
            var pathFile = path + "text.txt";
            Assert.IsFalse(client.Get(pathFile, path));
        }

        [TestMethod]
        public void DirectoryNonexistent()
        {
            var pathDir = path + "tss";
            Assert.AreEqual(0, client.List(pathDir).Count);
        }
    }
}
