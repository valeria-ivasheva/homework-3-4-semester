using System;
using SimpleFtpServer;
using SimpleFtpClient;
using System.IO;
using NUnit.Framework;
using System.Linq;

namespace FtpClientTests
{
    [TestFixture]
    public class ClientTest
    {
        public Server server = new Server(22234);
        private Client client = new Client("localhost", 22234);
        private Client clientOne = new Client("localhost", 22234);

        [OneTimeSetUp]
        public void Initialize()
        {
            server = new Server(22234);
            clientOne = new Client("localhost", 22234);
            client = new Client("localhost", 22234);
            server.Start();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            server.Stop();
        }

        //[ClassCleanup]
        //public static void CleanUp() => server.Stop();

        [Test]
        public void ConnectedCorrectlyTest()
        {
            Assert.IsTrue(client.Connect());
        }

        [Test]
        public void ListTest()
        {
            string path = Directory.GetCurrentDirectory();
            path = Directory.GetParent(path).FullName;
            path = Directory.GetParent(path).FullName;
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

        //[TestMethod]
        //public void GetTest()
        //{

        //}

        //[TestMethod]
        //public void Errror()
        //{ }
    }
}