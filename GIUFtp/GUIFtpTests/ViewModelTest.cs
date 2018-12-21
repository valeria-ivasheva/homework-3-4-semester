using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using GIUFtp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleFtpServer;

namespace GUIFtpTests
{
    [TestClass]
    public class ViewModelTest
    {
        private ViewModel viewModel;
        private Server server;
        private string pathToDir;

        [TestInitialize]
        public void Initialize()
        {
            viewModel = new ViewModel();
            server = new Server(22234);
            server.Start();
            viewModel.Server = "localhost";
            viewModel.Port = "22234";
            pathToDir = @"D:\Zona Downloads";
            viewModel.PathSave = @"D:\Zona Downloads\New";
            viewModel.CurrentPath = pathToDir;
            viewModel.ConnectCommand.Execute(this);
        }

        [TestMethod]
        public void ConnectTest()
        {
            viewModel.ConnectCommand.Execute(this);
            Assert.IsTrue(viewModel.IsConnected());
        }

        [TestMethod]
        public async System.Threading.Tasks.Task OpenTest()
        {
            //var pathTo = @"D:\summerSchool";
            //viewModel.CurrentPath = pathTo;
            await (viewModel.OpenCommand as CommandAsync).ExecuteAsync(this);
            int countDir = 0;
            int countFiles = 0;
            var dInfo = new DirectoryInfo(pathToDir);
            var directories = dInfo.GetDirectories().OrderBy(d => d.Name).ToList();
            var listDirName = directories.Select(d => d.Name);
            var files = dInfo.GetFiles().OrderBy(f => f.Name).ToList();
            var listFileName = files.Select(f => f.Name);
            Assert.AreEqual(11, viewModel.List.Count());
            for (int i = 0; i < viewModel.List.Count; i++)
            {
                var temp = viewModel.List[i].Name;
                if (viewModel.List[i].IsDir)
                {
                    countDir++;
                    Assert.IsTrue(listDirName.Contains(temp));
                }
                else
                {
                    countFiles++;
                    Assert.IsTrue(listFileName.Contains(temp));
                }
            }
            Assert.AreEqual(directories.Count, countDir);
            Assert.AreEqual(files.Count, countFiles);
            viewModel.CurrentPath = pathToDir;
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TryOpenTestAsync()
        {
            await (viewModel.OpenCommand as CommandAsync).ExecuteAsync(this);
            await viewModel.TryOpenFolder(viewModel.List[0]);
            Assert.AreEqual(@"D:\Zona Downloads\New", viewModel.CurrentPath);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task BackTest()
        {
            await (viewModel.OpenCommand as CommandAsync).ExecuteAsync(this);
            await viewModel.TryOpenFolder(viewModel.List[0]);
            viewModel.BackCommand.Execute(this);
            Assert.AreEqual(pathToDir, viewModel.CurrentPath);
        }

        [TestMethod]
        public void DisconnectTest()
        {
            viewModel.DisconnectCommand.Execute(this);
            Assert.IsFalse(viewModel.IsConnected());
        }

        [TestMethod]
        public async System.Threading.Tasks.Task DownloadTest()
        {
            var lines = new string[3] { "Test", "22222, 322, 23", "Olololo" };
            var pathFile = pathToDir + @"\MyFile.txt";
            using (var file = new StreamWriter(pathFile))
            {
                foreach (string line in lines)
                {
                    file.WriteLine(line);
                }
            }
            await (viewModel.OpenCommand as CommandAsync).ExecuteAsync(this);
            viewModel.SelectedListElement = new MyFile("MyFile.txt", false);
            await (viewModel.DownloadCommand as CommandAsync).ExecuteAsync(this);
            var pathSave = viewModel.PathSave + @"\MyFile.txt";
            using (var file = new StreamReader(pathSave))
            {
                for (int i = 0; i < 3; i++)
                {
                    Assert.AreEqual(lines[i], file.ReadLine());
                }
            }
            File.Delete(pathFile);
            File.Delete(pathSave);
            await (viewModel.OpenCommand as CommandAsync).ExecuteAsync(this);
            viewModel.SelectedListElement = new MyFile("lolo.txt", false);
            await (viewModel.DownloadCommand as CommandAsync).ExecuteAsync(this);
        }
    }
}
