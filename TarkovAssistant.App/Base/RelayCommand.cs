using System.Windows.Input;

namespace TarkovAssistant.App.Base
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Action<object?> execute) : this(execute, null) { }

        event EventHandler? ICommand.CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested += value;
        }

        void ICommand.Execute(object? parameter) => _execute(parameter);
        bool ICommand.CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;
    }
}
