using System.Windows.Input;

namespace MMU.BoerseDownloader.WpfUI.ViewModels.Shell
{
    public class ViewModelCommand
    {
        public ViewModelCommand(string displayName, ICommand command)
        {
            DisplayName = displayName;
            Command = command;
        }

        public ICommand Command { get; private set; }

        public string DisplayName { get; private set; }
    }
}