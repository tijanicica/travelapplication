using System;
using System.Windows.Input;
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.WPF.View;

public class DelegateCommand: ICommand
{
    
    private readonly Predicate<object> canExecutePredicate;
    private readonly Action<object> executionAction;


    public DelegateCommand(Action<object> execute)
        : this(execute, null)
    {
    }


    public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
    {
        if (execute == null) throw new ArgumentNullException("execute");

        executionAction = execute;
        canExecutePredicate = canExecute;
    }


    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object parameter)
    {
        return canExecutePredicate == null ? true : canExecutePredicate(parameter);
    }


    public void Execute(object parameter)
    {
        if (!CanExecute(parameter))
            throw new InvalidOperationException(
                "The command is not valid for execution, check the CanExecute method before attempting to execute.");

        executionAction(parameter);
    }
    
    
    public void RaiseCanExecuteChanged()
    {
        CommandManager.InvalidateRequerySuggested();
    }
}