using System;
using System.Collections.Generic;
using MMU.BoerseDownloader.WpfUI.ViewModels.Shell;

namespace MMU.BoerseDownloader.WpfUI.Handlers.NavigationHandling
{
    public interface INavigationHandlerConfiguration
    {
        IReadOnlyCollection<Action<ViewModelBase>> NavigationRequestedCallbacks { get; }

        void AddNavigationRequestedCallback(Action<ViewModelBase> navigationRequestedCallback);
    }
}