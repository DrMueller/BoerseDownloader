using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MMU.BoerseDownloader.WpfUI.Commands;
using MMU.BoerseDownloader.WpfUI.Handlers.ExceptionHandling;
using MMU.BoerseDownloader.WpfUI.Handlers.InformationHandling;
using MMU.BoerseDownloader.WpfUI.Handlers.NavigationHandling;
using MMU.BoerseDownloader.WpfUI.Interfaces;
using MMU.BoerseDownloader.WpfUI.Models;
using MMU.BoerseDownloader.WpfUI.Models.Enumerations;
using PropertyChanged;

namespace MMU.BoerseDownloader.WpfUI.ViewModels.Shell
{
    [ImplementPropertyChanged]
    internal class ViewModelContainer
    {
        private readonly INavigationHandler _navigationHandler;

        public ViewModelContainer(IExceptionHandlerConfiguration exceptionHandlerConfiguration, IInformationHandlerConfiguration informationHandlerConfiguration, INavigationHandlerConfiguration navigationHandlerConfiguration, INavigationHandler navigationHandler)
        {
            _navigationHandler = navigationHandler;

            exceptionHandlerConfiguration.AddExceptionCallback(ShowExceptionMessageCallback);
            informationHandlerConfiguration.AddInformationCallback(ShowInformationMessageAsyncCallback);
            navigationHandlerConfiguration.AddNavigationRequestedCallback(NavigateToViewModelCallback);

            _navigationHandler.NavigateTo<DownloadEntriesOverviewViewModel>(ViewModelParameterCollection.Empty);
        }

        public ParametredRelayCommand CloseCommand
        {
            get
            {
                return new ParametredRelayCommand(o =>
                {
                    var closable = (IClosable)o;
                    closable.Close();
                });
            }
        }

        public ViewModelBase CurrentContent { get; private set; }

        public string InformationText { get; private set; }

        public IEnumerable<ViewModelCommand> NavigationVmcs
        {
            get
            {
                return new List<ViewModelCommand>
                {
                    new ViewModelCommand("Activities", new RelayCommand(() =>
                    {
                        _navigationHandler.NavigateTo<DownloadEntriesOverviewViewModel>(ViewModelParameterCollection.Empty);
                    })),
                    new ViewModelCommand("Config Links", new RelayCommand(() =>
                    {
                        _navigationHandler.NavigateTo<DownloadContextsOverviewViewModel>(ViewModelParameterCollection.Empty);
                    })),
                    new ViewModelCommand("Config User", new RelayCommand(() =>
                    {
                        _navigationHandler.NavigateTo<BoerseUserEditViewModel>(ViewModelParameterCollection.Empty);
                    })),
                    new ViewModelCommand("Migration", new RelayCommand(() =>
                    {
                        _navigationHandler.NavigateTo<MigrationViewModel>(ViewModelParameterCollection.Empty);
                    }))
                };
            }
        }

        public InformationType SelectedInformationType { get; private set; }

        private void NavigateToViewModelCallback(ViewModelBase viewModelBase)
        {
            CurrentContent = viewModelBase;
        }

        private void PublishInformation(InformationType informationType, string message)
        {
            InformationText = message;
            SelectedInformationType = informationType;
        }

        private void ShowExceptionMessageCallback(Exception ex)
        {
            var text = ex.Message;
            PublishInformation(InformationType.Error, text);
        }

        private async void ShowInformationMessageAsyncCallback(InformationType informationType, string message, int? displaySeconds)
        {
            PublishInformation(informationType, message);
            if (displaySeconds.HasValue)
            {
                await Task.Delay(displaySeconds.Value * 1000);
                PublishInformation(InformationType.None, string.Empty);
            }
        }
    }
}