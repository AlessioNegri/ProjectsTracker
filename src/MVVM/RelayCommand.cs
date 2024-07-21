using System.Windows.Input;

namespace ProjectsTracker.src.MVVM
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute;

        private Func<object, bool>? can_execute;

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action<object> execute, Func<object, bool>? can_execute = null)
        {
            this.execute        = execute;
            this.can_execute    = can_execute;
        }

        public bool CanExecute(object? parameter) => can_execute is null || can_execute(parameter);

        public void Execute(object? parameter) =>  execute(parameter);
    }
}
