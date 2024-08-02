using System;
using System.Windows.Input;

namespace BookingApp;

public class ExecuteCommand<T> : ICommand
{
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        public ExecuteCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }


        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
    }

    public class ExecuteCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<bool> _canExecute;
        private readonly Predicate<object> _canExecute2;

        public ExecuteCommand(Action<object> execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public ExecuteCommand(Action<Object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new Exception("execute");
            _execute = execute;
            _canExecute2 = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    
}