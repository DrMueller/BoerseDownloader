using System;
using System.Collections.Generic;
using MMU.BoerseDownloader.Logics;
using MMU.BoerseDownloader.Model;
using MMU.BoerseDownloader.Model.Enumerations;
using MMU.BoerseDownloader.WpfUI.Commands;
using MMU.BoerseDownloader.WpfUI.Handlers.ExceptionHandling;
using MMU.BoerseDownloader.WpfUI.Handlers.NavigationHandling;
using MMU.BoerseDownloader.WpfUI.Models;
using MMU.BoerseDownloader.WpfUI.ViewModels.Shell;

namespace MMU.BoerseDownloader.WpfUI.ViewModels
{
    public class DownloadContextEditViewModel : ViewModelBase
    {
        private readonly IExceptionHandler _exceptionHandler;
        private readonly DownloadContextConfigurationLogic _logic;
        private readonly INavigationHandler _navigationHandler;

        public DownloadContextEditViewModel(IExceptionHandler exceptionHandler, INavigationHandler navigationHandler, DownloadContextConfigurationLogic logic)
        {
            BoerseLinkProviders = (BoerseLinkProvider[])Enum.GetValues(typeof(BoerseLinkProvider));
            _exceptionHandler = exceptionHandler;
            _navigationHandler = navigationHandler;
            _logic = logic;
        }

        public IEnumerable<BoerseLinkProvider> BoerseLinkProviders { get; }

        public ViewModelCommand CancelEdit
        {
            get
            {
                return new ViewModelCommand("Cancel", new RelayCommand(NavigateToOverview));
            }
        }

        public override string DisplayName => "Edit Boerse-Context";

        public BoerseLinkProvider DownloadEntryBoerseLinkProvider { get; set; }

        public long DownloadEntryId { get; private set; }

        public string DownloadEntryName { get; set; }

        public string DownloadEntryThreadUrl { get; set; }

        public ViewModelCommand SaveDownloadContextEntry
        {
            get
            {
                return new ViewModelCommand("Save", new RelayCommand(() =>
                    _exceptionHandler.HandledAction(() =>
                    {
                        _logic.Save(new DownloadContext
                        {
                            BoerseLinkProvider = DownloadEntryBoerseLinkProvider,
                            Id = DownloadEntryId,
                            Name = DownloadEntryName,
                            ThreadUrl = DownloadEntryThreadUrl
                        });

                        NavigateToOverview();
                    }), canExecute: CheckAllRequiredFields));
            }
        }

        public override void InjectParameters(ViewModelParameterCollection viewModelParameterCollection)
        {
            var downloadContext = (DownloadContext)viewModelParameterCollection["DownloadContext"].Value;
            DownloadEntryId = downloadContext.Id;
            DownloadEntryName = downloadContext.Name;
            DownloadEntryThreadUrl = downloadContext.ThreadUrl;
            DownloadEntryBoerseLinkProvider = downloadContext.BoerseLinkProvider;
        }

        private bool CheckAllRequiredFields()
        {
            return !string.IsNullOrEmpty(DownloadEntryName) && !string.IsNullOrEmpty(DownloadEntryThreadUrl);
        }

        private void NavigateToOverview()
        {
            _navigationHandler.NavigateTo<DownloadContextsOverviewViewModel>(ViewModelParameterCollection.Empty);
        }
    }
}