using System;
using System.Windows.Input;

namespace PWFramework_Mnogoletov
{
    public class RelayCommand : ICommand
    {
        private Action<Object> execute;
        private Func<Object, Boolean> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public Boolean CanExecute(Object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(Object parameter)
        {
            this.execute(parameter);
        }

        public RelayCommand(Action<Object> execute, Func<Object, Boolean> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
    }
}