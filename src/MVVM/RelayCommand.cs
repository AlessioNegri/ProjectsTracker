using System.Windows.Input;

namespace ProjectsTracker.src.MVVM
{
    /// <summary> Class to manage Relay Commands </summary>
    public class RelayCommand : ICommand
    {
        #region MEMBERS

        /// <summary> Execute delegate </summary>
        private Action<object> execute;

        /// <summary> Can Execute delegate </summary>
        private Func<object, bool>? can_execute;

        #endregion

        #region EVENTS

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Constructor </summary>
        /// <param name="execute"> Execute command </param>
        /// <param name="can_execute"> Can execute command </param>
        public RelayCommand(Action<object> execute, Func<object, bool>? can_execute = null)
        {
            this.execute        = execute;
            this.can_execute    = can_execute;
        }

        public bool CanExecute(object? parameter) => can_execute is null || can_execute(parameter);

        public void Execute(object? parameter) =>  execute(parameter);

        #endregion
    }
}