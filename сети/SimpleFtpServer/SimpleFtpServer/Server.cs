using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleServer
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
                //var data = await reader.ReadLineAsync();
                string str = await reader.ReadLineAsync();
                await ExecuteEcho(writer, str);
                //if (data == "Echo")
                //{
                //  await ExecuteEcho(writer, str);
                //}   
                //else
                //{
                //  string message = "Wrong command";
                //  await writer.WriteAsync(message);
                //}
                Disconnect(tcpClient);
            }
            catch (Exception e)
            {
                Disconnect(client as TcpClient);
            }
        }

        private async Task ExecuteEcho(StreamWriter writer, string str)
        {
            try
            {
              Console.WriteLine("Try send");
              await writer.WriteLineAsync(str);
            }
            catch (SocketException e)
            {
              Console.WriteLine(e.Message);
              writer.BaseStream.Dispose();
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
