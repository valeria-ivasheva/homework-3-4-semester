using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleFtpServer
{
    /// <summary>
    /// Сервер, обрабатывающий два запроса List и Get
    /// </summary>
    public class Server
    {
        private TcpListener listener;
        private readonly int port;
        private CancellationTokenSource cts = new CancellationTokenSource();

        public Server(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            this.port = port;
        }

        /// <summary>
        /// Запускает сервер
        /// </summary>
        public async Task Start()
        {
            listener.Start();
            Console.WriteLine($"Listening on port {port}...");
            while (!cts.IsCancellationRequested)
            {
                ThreadPool.QueueUserWorkItem(ProcessingRequest, await listener.AcceptTcpClientAsync());
            }
        }

        private async void ProcessingRequest(object client)
        {
            try
            {
                var tcpClient = client as TcpClient;
                NetworkStream stream = tcpClient.GetStream();
                var reader = new StreamReader(stream);
                var writer = new StreamWriter(stream) { AutoFlush = true };
                var data = await reader.ReadLineAsync();
                string path = await reader.ReadLineAsync();
                switch (data)
                {
                    case "2":
                        await ExecuteGet(writer, path);
                        break;
                    case "1":
                        await ExecuteList(writer, path);
                        break;
                    default:
                        {
                            string message = "Wrong command";
                            await writer.WriteAsync(message);
                            break;
                        }
                }
                Disconnect(tcpClient);
            }
            catch (IOException e)
            {            
                Disconnect(client as TcpClient);
            }
        }

        private async Task ExecuteGet(StreamWriter writer, string path)
        {
            try
            {
                var fileInfo = new FileInfo(path);
                Console.WriteLine(fileInfo.Exists);
                if (!fileInfo.Exists)
                {
                    await writer.WriteAsync("-1");
                    return;
                }
                using (FileStream file = File.OpenRead(path))
                {
                    string size = fileInfo.Length.ToString();
                    await writer.WriteLineAsync(size);
                    await file.CopyToAsync(writer.BaseStream);
                    Console.WriteLine("Send it");
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
                writer.BaseStream.Dispose();
            }
            catch (ArgumentNullException)
            {
                await writer.WriteAsync("-1");
                writer.BaseStream.Dispose();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                writer.BaseStream.Dispose();
            }
        }

        private async Task ExecuteList(StreamWriter writer, string path)
        {
            DirectoryInfo directoryInfo;
            try
            {
                directoryInfo = new DirectoryInfo(path);
            }
            catch (ArgumentException)
            {
                await writer.WriteAsync("-1");
                return;
            }
            if (!directoryInfo.Exists)
            { 
                await writer.WriteAsync("-1");
                return;
            }
            FileInfo[] files = directoryInfo.GetFiles();
            DirectoryInfo[] directories = directoryInfo.GetDirectories();
            var size = (files.Length + directories.Length).ToString();
            writer.WriteLine(size);
            for (int i = 0; i < directories.Length; i++)
            {
                writer.WriteLine($"{directories[i].Name} true");
            }
            for (int i = 0; i < files.Length; i++)
            {
                writer.WriteLine($"{files[i].Name} false");
            }
        }

        private void Disconnect(TcpClient client)
        {
            client.Close();
        }

        /// <summary>
        /// Останавливает сервер
        /// </summary>
        public void Stop()
        {
            cts.Cancel();
        }
    }
}
