using System;
using System.Windows.Input;

namespace MMU.BoerseDownloader.WpfUI.Commands
{
    public class ParametredRelayCommand : ICommand
    {
        private readonly Action<object> _action;
        private readonly Func<bool> _canExecute;

        public ParametredRelayCommand(Action<object> action)
            : this(action, null)
        {
        }

        public ParametredRelayCommand(Action<object> action, Func<bool> canExecute)
        {
            _canExecute = canExecute;
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}