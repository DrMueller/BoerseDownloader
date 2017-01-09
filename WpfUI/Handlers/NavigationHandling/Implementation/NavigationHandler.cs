using Microsoft.Practices.ObjectBuilder2;
using MMU.BoerseDownloader.WpfUI.Models;
using MMU.BoerseDownloader.WpfUI.ViewModels.Shell;

namespace MMU.BoerseDownloader.WpfUI.Handlers.NavigationHandling.Implementation
{
    public class NavigationHandler : INavigationHandler
    {
        private readonly INavigationHandlerConfiguration _navigationHandlerConfiguration;
        private readonly ViewModelFactory _viewModelFactory;

        public NavigationHandler(INavigationHandlerConfiguration navigationHandlerConfiguration, ViewModelFactory viewModelFactory)
        {
            _navigationHandlerConfiguration = navigationHandlerConfiguration;
            _viewModelFactory = viewModelFactory;
        }

        public void NavigateTo<T>(ViewModelParameterCollection viewModelParameterCollection)
            where T : ViewModelBase
        {
            var targetViewModel = _viewModelFactory.CreateViewModel<T>(viewModelParameterCollection);
            _navigationHandlerConfiguration.NavigationRequestedCallbacks.ForEach(f => f.Invoke(targetViewModel));
        }
    }
}