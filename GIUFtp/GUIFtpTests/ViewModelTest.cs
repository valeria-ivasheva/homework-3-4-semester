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
            pathToDir = Directory.GetCurrentDirectory();
            Directory.CreateDirectory(pathToDir + @"\New");
            viewModel.PathSave = pathToDir + @"\New"; 
            viewModel.CurrentPath = pathToDir;
        }

        [TestMethod]
        public async System.Threading.Tasks.Task ConnectTest()
        {
            await (viewModel.ConnectCommand as CommandAsync).ExecuteAsync(this);
            Assert.IsTrue(viewModel.IsConnected());
        }

        [TestMethod]
        public async System.Threading.Tasks.Task OpenTest()
        {
            await (viewModel.ConnectCommand as CommandAsync).ExecuteAsync(this);
            await (viewModel.OpenCommand as CommandAsync).ExecuteAsync(this);
            int countDir = 0;
            int countFiles = 0;
            var dInfo = new DirectoryInfo(pathToDir);
            var directories = dInfo.GetDirectories().OrderBy(d => d.Name).ToList();
            var listDirName = directories.Select(d => d.Name);
            var files = dInfo.GetFiles().OrderBy(f => f.Name).ToList();
            var listFileName = files.Select(f => f.Name);
            Assert.AreEqual(directories.Count + files.Count, viewModel.List.Count());
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
            await (viewModel.ConnectCommand as CommandAsync).ExecuteAsync(this);
            await (viewModel.OpenCommand as CommandAsync).ExecuteAsync(this);
            await viewModel.TryOpenFolder(viewModel.List[0]);
            var folder = new DirectoryInfo(pathToDir).GetDirectories().OrderBy(b => b.Name).First();
            Assert.AreEqual(pathToDir + @"\" + folder.Name, viewModel.CurrentPath);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task BackTest()
        {
            await (viewModel.ConnectCommand as CommandAsync).ExecuteAsync(this);
            await (viewModel.OpenCommand as CommandAsync).ExecuteAsync(this);
            await viewModel.TryOpenFolder(viewModel.List[0]);
            viewModel.BackCommand.Execute(this);
            Assert.AreEqual(pathToDir, viewModel.CurrentPath);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task DisconnectTest()
        {
            await (viewModel.ConnectCommand as CommandAsync).ExecuteAsync(this);
            viewModel.DisconnectCommand.Execute(this);
            Assert.IsFalse(viewModel.IsConnected());
        }

        [TestMethod]
        public async System.Threading.Tasks.Task DownloadTest()
        {
            await (viewModel.ConnectCommand as CommandAsync).ExecuteAsync(this);
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
