using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GIUFtp
{
    /// <summary>
    /// Интерфейс ассинхронных команд
    /// </summary>
    public interface IAsyncCommand : ICommand
    {
        /// <summary>
        /// Выполнить ассинхронно команду
        /// </summary>
        Task ExecuteAsync(object parameter);
    }
}
