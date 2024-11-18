using System;
using System.Windows.Input;

namespace Prog7312POEST10071737.Core
{
    /// <summary>
    /// A command implementation that can be used to bind methods to UI controls.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private Action<object> _execute;

        private Func<object, bool> _canExecute;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Occurs when the ability to execute the command changes.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The method to be executed when the command is invoked.</param>
        /// <param name="canExecute">The method that determines whether the command can be executed.</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Determines whether the command can be executed.
        /// </summary>
        /// <param name="parameter">The parameter passed to the command.</param>
        /// <returns><c>true</c> if the command can be executed; otherwise, <c>false</c>.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The parameter passed to the command.</param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
//____________________________________EOF_________________________________________________________________________