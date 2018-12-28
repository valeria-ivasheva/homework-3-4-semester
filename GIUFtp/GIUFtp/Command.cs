using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GIUFtp
{
    /// <summary>
    /// Класс команды
    /// </summary>
    public class Command : ICommand
    {
        private readonly Action command;
        private readonly Func<bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="command"> Действие для выполнения</param>
        public Command(Action command, Func<bool> canExecute = null)
        {
            this.canExecute = canExecute;
            this.command = command ?? throw new ArgumentNullException();
        }

       /// <summary>
       /// Можно ли выполнить команду
       /// </summary>
        public bool CanExecute(object parameter) 
        {
            return canExecute?.Invoke() ?? true;
        }

        /// <summary>
        /// Выполнить команду
        /// </summary>
        public void Execute(object parameter) => command();
    }
}
