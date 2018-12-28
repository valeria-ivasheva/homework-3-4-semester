using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GIUFtp
{
    /// <summary>
    /// ViewModel для GUI
    /// </summary>
    public class ViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private int port;
        private string server;
        private MainWindow window;
        private Client client;
        private bool connected;
        private string rootFolder;
        private string pathSave;
        private string currentPath;

        /// <summary>
        /// Порт для сервера
        /// </summary>
        public string Port
        {
            get => Convert.ToString(port);
            set
            {
                if (!connected)
                {
                    try
                    {
                        port = Convert.ToInt32(value);
                    }
                    catch (FormatException e)
                    {
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Адрес для попдключения
        /// </summary>
        public string Server
        {
            get => server;
            set
            {
                if (!connected)
                {
                    server = value;
                }
            }
        }

        /// <summary>
        /// Путь для сохранения
        /// </summary>
        public string PathSave
        {
            get => pathSave;
            set
            {
                pathSave = value;
                NotifyPropertyChanged("PathSave");
            }
        }

        /// <summary>
        /// Путь к каталогу, отображаемому в данный момент
        /// </summary>
        public string CurrentPath
        {
            get => currentPath;
            set
            {
                currentPath = value;
                NotifyPropertyChanged("CurrentPath");
            }
        }

        /// <summary>
        /// Список папок, на которые смотрит сервер
        /// </summary>
        public ObservableCollection<MyFile> List { get; set; }

        /// <summary>
        /// Список загружаемых/загруженных файлов
        /// </summary>
        public ObservableCollection<MyFile> ListDownload { get; set; }

        /// <summary>
        /// Выбранный элемент для загрузки
        /// </summary>
        public MyFile SelectedListElement { get; set; }

        public ViewModel(MainWindow window)
        {
            this.window = window;
            server = "localhost";
            port = 22234;
            CurrentPath = @"";
            rootFolder = CurrentPath;
            connected = false;
            List = new ObservableCollection<MyFile>();
            ListDownload = new ObservableCollection<MyFile>();
        }

        public ViewModel()
        {
            rootFolder = CurrentPath;
            connected = false;
            List = new ObservableCollection<MyFile>();
            ListDownload = new ObservableCollection<MyFile>();
        }

        /// <summary>
        /// Команда подключения к серверу
        /// </summary>
        public ICommand ConnectCommand => new CommandAsync(Connect);

        /// <summary>
        /// Команда возвращения назад по папке
        /// </summary>
        public ICommand BackCommand => new Command(Back);

        /// <summary>
        /// Команда отключения от сервера
        /// </summary>
        public ICommand DisconnectCommand => new Command(Disconnect);

        /// <summary>
        /// Команда загрузки
        /// </summary>
        public ICommand DownloadCommand => new CommandAsync(Download);

        /// <summary>
        /// Команда открытия корневого каталога
        /// </summary>
        public ICommand OpenCommand => new CommandAsync(OpenRoot);

        /// <summary>
        /// Открыть директорию
        /// </summary>
        /// <param name="selectedElement"> Открываемый элемент</param>
        public async Task TryOpenFolder(MyFile selectedElement)
        {
            if (!connected || selectedElement == null)
            {
                return;
            }
            if (!selectedElement.IsDir)
            {
                return;
            }
            var path = CurrentPath + @"\" + selectedElement.Name;
            CurrentPath = path;
            var listGet = await client.List(path);
            if (listGet != null)
            {
                List.Clear(); 
                foreach (var tempElement in listGet)
                {
                    List.Add(new MyFile(tempElement.Name, tempElement.IsDir));
                }
            }
        }

        /// <summary>
        /// Проверяет подключено ли к серверу
        /// </summary>
        /// <returns> True, если подключено</returns>
        public bool IsConnected()
        {
            return connected;
        }

        private async Task Connect()
        {
            await Task.Run(() =>
            {
                client = new Client(Server, port);
                connected = client.Connect();
            });
            if (connected)
            {
                if (window != null)
                {
                    window.ServerBox.IsReadOnly = true;
                    window.PortBox.IsReadOnly = true;
                }
                ShowBox($"Подключено к серверу {Server}", "Состояние подключения");
            }
            else
            {
                ShowBox($"Не удалось подключиться к серверу {Server}, попробуйте еще раз", "Состояние подключения");
            }
        }

        private void ShowBox(string message, string name)
        {
            MessageBox.Show(message, name, MessageBoxButton.OK);
        }

        private async Task OpenRoot()
        {
            if (!connected)
            {
                ShowBox("Подключитесь к серверу", "Отсутствует подключение к серверу");
                return;
            }
            rootFolder = CurrentPath;
            if (CurrentPath == "")
            {
                ShowBox("Введите, пожалуйста, путь к корневому каталогу", "Не найден путь");
                return;
            }
            await Open(CurrentPath);
        }

        private async Task Open(string path)
        {
            CurrentPath = path;
            var listGet = await client.List(path);
            if (listGet != null)
            {
                List.Clear();
                foreach (var tempElement in listGet)
                {
                    List.Add(new MyFile(tempElement.Name, tempElement.IsDir));
                }
            }
        }

        private void Disconnect()
        {
            client.Close();
            connected = false;
            CurrentPath = "";
            PathSave = "";
            if (window != null)
            {
                window.ServerBox.IsReadOnly = false;
                window.PortBox.IsReadOnly = false;
            }
            ListDownload.Clear();
            List.Clear();
        }

        private async void Back()
        {
            if (!connected || CurrentPath == rootFolder)
            {
                return;
            }
            string path = CurrentPath.Substring(0, CurrentPath.LastIndexOf(@"\"));
            await Open(path);
        }

        private async Task Download()
        {
            if (PathSave == "")
            {
                ShowBox("Введите путь для сохранения", "Не найден путь для сохранения");
                return;
            }
            ListDownload.Clear();
            if (SelectedListElement != null)
            {
                if (SelectedListElement.IsDir)
                {
                    return;
                }
                SelectedListElement.ImagePath = "images.png";
                ListDownload.Add(SelectedListElement);
            }
            if (SelectedListElement == null)
            {
                MessageBoxResult result = MessageBox.Show($"Хотите скачать все файлы из папки?", "Загрузка файлов", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    foreach (var temp in List)
                    {
                        if (!temp.IsDir)
                        {
                            temp.ImagePath = "images.png";
                            ListDownload.Add(temp);
                        }
                    }
                }
                else
                {
                    return;
                }
            }
            var tasks = ListDownload.Select(i => DownloadFiles(i));
            await Task.WhenAll(tasks);
        }

        private async Task DownloadFiles(MyFile currentFile)
        {
            var path = CurrentPath + @"\" + currentFile.Name;
            var savePath = PathSave + @"\" + currentFile.Name;
            var resultGet = await client.Get(path, savePath);
            if (resultGet)
            {
                currentFile.ImagePath = "ok.png";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
