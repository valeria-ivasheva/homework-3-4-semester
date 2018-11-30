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

namespace TicTacToe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game game;
        private bool start;
        private string user;
        private string userTwo;
        private int queue;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonStartClick(object sender, RoutedEventArgs e)
        {
            Reset();
            start = true;
            var result = MessageBox.Show("Первый игрок будет играть крестиками?", "Выберете", MessageBoxButton.OKCancel);
            user = (result == MessageBoxResult.OK) ? "x" : "o";
            userTwo = (user == "x") ? "o" : "x";
            game = new Game(user);
        }

        private void buttonClick(object sender, RoutedEventArgs e)
        {
            if (!start)
            {
                MessageBox.Show("Нажмите на старт", "Пожалуйста", MessageBoxButton.OK);
                return;
            }
            queue++;
            bool moveOne = queue % 2 == 1;
            ButtonContent(sender, moveOne);
            string temp = (sender as Button).Name;
            int number;
            int.TryParse(temp.Substring(temp.Length - 1), out number);
            game.InputElement(number - 1);
            if (moveOne && game.IsUserOneWin())
            {
                MessageBox.Show("Еееее, первый игрок выиграл", "Поздравляем", MessageBoxButton.OK);
                Reset();
                game.Reset();
                return;
            }
            if (queue == 9)
            {
                MessageBox.Show("У нас ничья", "Поздравляем", MessageBoxButton.OK);
                Reset();
                game.Reset();
                return;
            }
            if (!moveOne && game.IsUserTwoWin())
            {
                MessageBox.Show("Еееее, второй игрок выиграл", "Поздравляем", MessageBoxButton.OK);
                Reset();
                game.Reset();
                return;
            }
        }

        private void ButtonContent(object sender, bool isUserOne)
        {
            if ((isUserOne && user == "x") || (!isUserOne && user == "o"))
            {
                (sender as Button).Content = "X";
            }
            if ((isUserOne && user == "o") || (!isUserOne && user == "x"))
            {
                (sender as Button).Content = "O";
            }
            (sender as Button).IsEnabled = false;
        }

        private void Reset()
        {
            button1.Content = "";
            button2.Content = "";
            button3.Content = "";
            button4.Content = "";
            button5.Content = "";
            button6.Content = "";
            button7.Content = "";
            button8.Content = "";
            button9.Content = "";
            button1.IsEnabled = true;
            button2.IsEnabled = true;
            button3.IsEnabled = true;
            button4.IsEnabled = true;
            button5.IsEnabled = true;
            button6.IsEnabled = true;
            button7.IsEnabled = true;
            button8.IsEnabled = true;
            button9.IsEnabled = true;
            user = "";
            queue = 0;
            start = false;
            userTwo = "";
        }
    }
}
