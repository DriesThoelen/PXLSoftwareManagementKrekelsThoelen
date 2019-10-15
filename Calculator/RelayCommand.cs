using System;
using System.Windows.Input;

namespace Calculator
{
    public class RelayCommand : GalaSoft.MvvmLight.Command.RelayCommand, ICommand
    {

        public RelayCommand(Action execute, Func<bool> canExecute, bool keepTargetAlive = false) : base(execute, canExecute, keepTargetAlive)
        {
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                base.CanExecuteChanged += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                base.CanExecuteChanged -= value;
            }
        }
    }
}