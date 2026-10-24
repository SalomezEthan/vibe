using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace vibe.Services
{
    public class RelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool>? canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return this.canExecute is null || this.canExecute.Invoke();
        }

        public void Execute(object? parameter)
        {
            this.execute.Invoke();
        }

        public event EventHandler? CanExecuteChanged;
    }

    public class RelayCommandAsync : ICommand
    {
        private readonly Func<Task> execute;
        private readonly Func<bool>? canExecute;

        public RelayCommandAsync(Func<Task> execute, Func<bool>? canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return this.canExecute is null || this.canExecute.Invoke();
        }

        public async void Execute(object? parameter)
        {
            await this.execute.Invoke();
        }
        public event EventHandler? CanExecuteChanged;
    }
}
