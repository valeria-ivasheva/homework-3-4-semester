using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GIUFtp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var context = new ViewModel(this);
            DataContext = context;

        }

        private async void SelectionItemChoose(object sender, SelectionChangedEventArgs e)
        {
            var selectedElement = (sender as ListBox).SelectedItem as MyFile;
            await (DataContext as ViewModel).TryOpenFolder(selectedElement);            
        }
    }
}
