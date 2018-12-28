using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GIUFtp
{
    /// <summary>
    /// Класс, реализующий ассинхронную команду
    /// </summary>
    public class CommandAsync : IAsyncCommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private bool isExecuting;
        private readonly Func<Task> execute;
        private readonly Func<bool> canExecute;

        private readonly Func<Task> command;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="command"> Команда, которую нужно выполнить ассинхронно</param>
        public CommandAsync(Func<Task> command)
        {
            this.command = command;
        }

        /// <summary>
        /// Можно либо выполнить команду
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Выполнить команду
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Task ExecuteAsync(object parameter)
        {
            return command();
        }

        /// <summary>
        /// Выполнить команду ассинхронно
        /// </summary>
        /// <param name="parameter"></param>
        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
