using MMU.BoerseDownloader.WpfUI.Models;
using MMU.BoerseDownloader.WpfUI.ViewModels.Shell;

namespace MMU.BoerseDownloader.WpfUI.Handlers.NavigationHandling
{
    public interface INavigationHandler
    {
        void NavigateTo<T>(ViewModelParameterCollection viewModelParameterCollection)
            where T : ViewModelBase;
    }
}