using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace SimpleFtpClient
{
    class Client
    {
        private TcpClient client;
        private readonly int port;

        public Client(string server, int port)
        {
            client = new TcpClient(server, port);
            if (client.Connected)
            {
                {
                    Console.WriteLine("Connected");
                }
            }
            this.port = port;
        }

        public bool Get(string path)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                var reader = new StreamReader(stream);
                var writer = new StreamWriter(stream);
                writer.WriteLineAsync("2");
                writer.WriteLineAsync(path);
                var size = Convert.ToInt32(reader.ReadLineAsync());
                if (size == -1)
                {
                    Console.WriteLine("File doesn't exists");
                    return false;
                }
                var savePath = Console.ReadLine();
                var fileResult = new FileStream(savePath, FileMode.Create);
                var buffer = new byte[size];
                int bytesRead;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fileResult.Write(buffer, 0, bytesRead);
                }
                return true;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
                return false;
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                return false;
            }
            catch (IOException e)
            {
                Console.WriteLine("IOException: {0}", e);
                return false;
            }
        }

        public List<MyFile> List(string path)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                var reader = new StreamReader(stream);
                var writer = new StreamWriter(stream);
                var result = new List<MyFile>();
                writer.WriteLineAsync("1");
                writer.WriteLineAsync(path);
                var count = reader.Read();
                Console.WriteLine($"Size of directory {count}");
                for (int i = 0; i < count; i++)
                {
                    var str = reader.ReadLine();
                    result[i] = new MyFile(str);
                }
                return result;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
                return null;
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                return null;
            }
            catch (IOException e)
            {
                Console.WriteLine("IOException: {0}", e);
                return null;
            }
        }
    }
}
