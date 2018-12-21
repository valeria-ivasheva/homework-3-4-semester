using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GIUFtp
{
    /// <summary>
    /// 
    /// </summary>
    public class MyFile : System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Имя файла
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Это директория
        /// </summary>
        public bool IsDir { get; private set; }

        /// <summary>
        /// Ссылка на изображение загрузки
        /// </summary>
        public string ImagePath
        {
            get
            { return imagePath; }
            set
            {
                imagePath = value;
                NotifyPropertyChanged("ImagePath");
            }
        }
       
        private string imagePath;

        public MyFile(string name, bool isDir)
        {
            Name = name;
            IsDir = isDir;
            imagePath = "image.png";
        }

        public MyFile(string str)
        {
            try
            {
                var strTemp = str.Split(' ');
                Name = strTemp[0];
                IsDir = Convert.ToBoolean(strTemp[1]);
            }
            catch(FormatException e)
            {
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
