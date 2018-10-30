using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleFtpServer
{
    class Server
    {
        private TcpListener listener;
        private Socket socket;
        private readonly int port;

        public Server(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            this.port = port;
        }

        public void Start()
        {
            listener.Start();
            Console.WriteLine($"Listening on port {port}...");
            while (true)
            {
                ThreadPool.QueueUserWorkItem(ProcessingRequest, listener.AcceptTcpClient());
            }
        }

        private async void ProcessingRequest(object client)
        {
            var tcpClient = client as TcpClient;
            NetworkStream stream = tcpClient.GetStream();
            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream);
            var data = await reader.ReadLineAsync();
            string path;
            switch (data)
            {
                case "2":
                    path = await reader.ReadLineAsync();
                    reader.Close();
                    ExecuteGet(writer, path);
                    break;
                case "1":
                    path = await reader.ReadLineAsync();
                    reader.Close();
                    ExecuteList(writer, path);
                    break;
                default:
                    {
                        string message = "Неправильная команда";
                        reader.Close();
                        writer.AutoFlush = true;
                        await writer.WriteAsync(message);
                        writer.Close();
                        return;
                    }
            }
        }

        private async void ExecuteGet(StreamWriter writer, string path)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(path);
                if (!fileInfo.Exists)
                {
                    writer.AutoFlush = true;
                    await writer.WriteAsync("-1");
                    return;
                }
                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    string size = fileInfo.Length.ToString();
                    writer.AutoFlush = true;
                    await writer.WriteLineAsync(size);
                    var content = new byte[fileInfo.Length];
                    int temp = file.Read(content, 0, (int)fileInfo.Length);
                    socket = listener.AcceptSocket();
                    socket.Send(content, 0, (int)fileInfo.Length, SocketFlags.None);
                    file.Close();
                }
                writer.Close();
            }
            catch (SocketException e)
            {
                
            }
            catch (ArgumentNullException e)
            {

            }
            catch (IOException e)
            {

            }
        }

        private async void ExecuteList(StreamWriter writer, string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
            {
                writer.AutoFlush = true;
                await writer.WriteAsync("-1");
                return;
            }
            FileInfo[] files = directoryInfo.GetFiles();
            DirectoryInfo[] directories = directoryInfo.GetDirectories();
            var size = (files.Length + directories.Length).ToString();
            await writer.WriteLineAsync(size);
            for (int i = 0; i < directories.Length; i++)
            {
                await writer.WriteLineAsync($"{directories[i].Name} true");
            }
            for (int i = 0; i < files.Length; i++)
            {
                await writer.WriteLineAsync($"{files[i].Name} false");
            }
        }

        public void Stop()
        {
            if (listener != null)
            {
                listener.Stop();
            }
        }
    }
}
