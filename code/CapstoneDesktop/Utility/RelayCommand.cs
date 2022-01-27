using System;
using System.Windows.Input;

namespace CapstoneDesktop.Utility
{
    /// <summary>
    ///     RelayCommand Class for View Utility
    /// </summary>
    /// <seealso cref="System.Windows.Input.ICommand" />
    public class RelayCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        /// <summary>Initializes a new instance of the <see cref="RelayCommand" /> class.</summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }


        /// <summary>Defines the method that determines whether the command can execute in its current state.</summary>
        /// <param name="parameter">
        ///     Data used by the command.  If the command does not require data to be passed, this object can
        ///     be set to null
        /// </param>
        /// <returns> true if the command can execute, false otherwise </returns>
        public bool CanExecute(object parameter)
        {
            var result = _canExecute?.Invoke(parameter) ?? true;
            return result;
        }


        /// <summary>
        ///     Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command.  If the command does not require data to be passed, this object can
        ///     be set to null
        /// </param>
        public void Execute(object parameter)
        {
            if (CanExecute(parameter)) _execute(parameter);
        }


        /// <summary>
        ///     Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler? CanExecuteChanged;


        /// <summary>
        ///     Called when [can execute changed].
        /// </summary>
        public virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}