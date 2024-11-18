using System;
using System.Windows.Input;

namespace Prog7312POEST10071737.Core
{
    public class RelayCommand : ICommand
    {
        private Action<object> _execute;

        private Func<object, bool> _canExecute;
        //___________________________________________________________________________________________________________

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        //___________________________________________________________________________________________________________

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        //___________________________________________________________________________________________________________

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }
        //___________________________________________________________________________________________________________

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
//____________________________________EOF_________________________________________________________________________