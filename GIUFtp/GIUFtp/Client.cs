using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GIUFtp
{
    public class Client
    {
        private TcpClient client;
        private readonly string server;
        private readonly int port;

        public Client(string server, int port)
        {
            this.port = port;
            this.server = server;
        }

        public void Close() => client.Close();

        /// <summary>
        /// Проверка подключился ли клиент к серверу
        /// </summary>
        /// <returns> True - все ок, false - что-то пошла не так</returns>
        public bool Connect()
        {
            try
            {
                client = new TcpClient(server, port);
                return client.Connected;
            }
            catch (SocketException)
            {
                return false;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        /// <summary>
        /// Получить файл
        /// </summary>
        /// <param name="path">  Путь к файлу</param>
        /// <param name="savePath"> Путь для сохранения</param>
        /// <returns> True - все удачно сохранилось, False - что-то пошло не так</returns>
        public async Task<bool> Get(string path, string savePath)
        {
            if (!Connect())
            {
                return false;
            }
            try
            {
                NetworkStream stream = client.GetStream();
                var writer = new StreamWriter(stream) { AutoFlush = true };
                writer.WriteLine("2");
                writer.WriteLine(path);
                var reader = new StreamReader(stream);
                var strSize = reader.ReadLineAsync();
                var size = long.Parse(strSize.Result);
                if (size == -1)
                {
                    Console.WriteLine("File doesn't exists");
                    return false;
                }
                Console.WriteLine(size);
                var fileResult = new FileStream(savePath, FileMode.Create);
                await reader.BaseStream.CopyToAsync(fileResult);
                Console.WriteLine("Get it");
                fileResult.Flush();
                fileResult.Close();
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

        /// <summary>
        /// Листинг файлов и деркторий, находящихся в path
        /// </summary>
        /// <param name="path"> Путь к директории, информацию о которой мы хотим найти</param>
        /// <returns> Количество файлов и папок, список из названий. Если такой папки нет, возвращает -1</returns>
        public async Task<List<MyFile>> List(string path)
        {
            if (!Connect())
            {
                return null;
            }
            try
            {
                NetworkStream stream = client.GetStream();
                var writer = new StreamWriter(stream) { AutoFlush = true };
                var result = new List<MyFile>();
                writer.WriteLine("1");
                Console.WriteLine("Отправили запрос List");
                writer.WriteLine(path);
                var reader = new StreamReader(stream);
                var strCount = await reader.ReadLineAsync();
                var count = int.Parse(strCount);
                Console.WriteLine($"Size of directory {count}");
                for (int i = 0; i < count; i++)
                {
                    var str = await reader.ReadLineAsync();
                    result.Add(new MyFile(str));
                }
                reader.Close();
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
