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
    /// 
    /// </summary>
    public class CommandAsync : IAsyncCommand
    {
        /// <summary>
        /// 
        /// </summary>
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
        /// 
        /// </summary>
        /// <param name="command"></param>
        public CommandAsync(Func<Task> command)
        {
            this.command = command;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Task ExecuteAsync(object parameter)
        {
            return command();
        }

        /// <summary>
        /// 
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
