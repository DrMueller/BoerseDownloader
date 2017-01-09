using System.Collections.ObjectModel;
using MMU.BoerseDownloader.Logics;
using MMU.BoerseDownloader.Model;
using MMU.BoerseDownloader.WpfUI.Commands;
using MMU.BoerseDownloader.WpfUI.Handlers.ExceptionHandling;
using MMU.BoerseDownloader.WpfUI.Handlers.NavigationHandling;
using MMU.BoerseDownloader.WpfUI.Models;
using MMU.BoerseDownloader.WpfUI.ViewModels.Shell;

namespace MMU.BoerseDownloader.WpfUI.ViewModels
{
    public class DownloadContextsOverviewViewModel : ViewModelBase
    {
        private readonly DownloadContextConfigurationLogic _downloadContextConfigurationLogic;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly INavigationHandler _navigationHandler;

        public DownloadContextsOverviewViewModel(IExceptionHandler exceptionHandler, INavigationHandler navigationHandler, DownloadContextConfigurationLogic downloadContextConfigurationLogic)
        {
            _exceptionHandler = exceptionHandler;
            _navigationHandler = navigationHandler;
            _downloadContextConfigurationLogic = downloadContextConfigurationLogic;
            DownloadContextEntries = new ObservableCollection<DownloadContext>(_downloadContextConfigurationLogic.LoadAll());
        }

        public ViewModelCommand DeleteDownloadContextEntry
        {
            get
            {
                return new ViewModelCommand("Delete", new RelayCommand(() =>
                    {
                        _downloadContextConfigurationLogic.Delete(SelectedDownloadContextEntry.Id);
                        DownloadContextEntries.Remove(SelectedDownloadContextEntry);
                        SelectedDownloadContextEntry = null;
                    },
                    canExecute: () => SelectedDownloadContextEntry != null));
            }
        }

        public override string DisplayName => "Boerse-Contexts Overview";

        public ObservableCollection<DownloadContext> DownloadContextEntries { get; }

        public ViewModelCommand EditDownloadContextEntry
        {
            get
            {
                return new ViewModelCommand("Edit", new RelayCommand(() =>
                    {
                        _exceptionHandler.HandledAction(() =>
                        {
                            var param = new ViewModelParameter("DownloadContext", SelectedDownloadContextEntry);
                            var paramCollection = new ViewModelParameterCollection(param);
                            _navigationHandler.NavigateTo<DownloadContextEditViewModel>(paramCollection);
                        });
                    },
                    canExecute: () => SelectedDownloadContextEntry != null));
            }
        }

        public ViewModelCommand NewDownloadContextEntry
        {
            get
            {
                return new ViewModelCommand("New", new RelayCommand(() =>
                {
                    _exceptionHandler.HandledAction(() =>
                    {
                        _navigationHandler.NavigateTo<DownloadContextEditViewModel>(ViewModelParameterCollection.Empty);
                    });
                }));
            }
        }

        public DownloadContext SelectedDownloadContextEntry { get; set; }
    }
}