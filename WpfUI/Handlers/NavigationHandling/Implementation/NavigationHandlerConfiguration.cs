using System;
using System.Collections.Generic;
using MMU.BoerseDownloader.WpfUI.ViewModels.Shell;

namespace MMU.BoerseDownloader.WpfUI.Handlers.NavigationHandling.Implementation
{
    public class NavigationHandlerConfiguration : INavigationHandlerConfiguration
    {
        private readonly List<Action<ViewModelBase>> _navigationRequestedCallbacks = new List<Action<ViewModelBase>>();

        public void AddNavigationRequestedCallback(Action<ViewModelBase> navigationRequestedCallback)
        {
            _navigationRequestedCallbacks.Add(navigationRequestedCallback);
        }

        public IReadOnlyCollection<Action<ViewModelBase>> NavigationRequestedCallbacks => _navigationRequestedCallbacks;
    }
}