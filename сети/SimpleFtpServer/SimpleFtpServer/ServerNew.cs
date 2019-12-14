using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SimpleFtpServer
{
  public class ServerNew
  {
    /// <summary>
    /// Главный метод, реализующий логику работы сервера.
    /// При попытке подключения клиента создается и запускается новый Task,
    /// в котором выполняется обработка запросов, поступающих с клиента.
    /// </summary>
    /// <param name="port">Номер порта.</param>
    public async Task Work(int port)
    {
      var listener = new TcpListener(IPAddress.Any, port);
      listener.Start();

      while (true)
      {
        var socket = await listener.AcceptSocketAsync();
        var manager = new Task(clientSocket => ManageRequest((Socket)clientSocket), socket);
        manager.Start();
      }
    }

    /// <summary>
    /// Метод позволяет корректно обрабатывать команды подключившегося клиента.
    /// Распознает тип команды и вызывает метод, требуемый для выполнения задачи.
    /// </summary>
    private async void ManageRequest(Socket socket)
    {
      var stream = new NetworkStream(socket);

      var reader = new StreamReader(stream);
      var request = await reader.ReadLineAsync();

      while (request != "exit")
      {
        System.Console.WriteLine(request);

        var writer = new StreamWriter(stream);

        await writer.WriteLineAsync(request);
        await writer.FlushAsync();

        request = await reader.ReadLineAsync();
      }


      socket.Close();
      return;
    }
  }
}
