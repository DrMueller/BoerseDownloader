using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MMU.BoerseDownloader.WpfUI.Commands;
using MMU.BoerseDownloader.WpfUI.Handlers.ExceptionHandling;
using MMU.BoerseDownloader.WpfUI.Handlers.InformationHandling;
using MMU.BoerseDownloader.WpfUI.Infrastructure.Extensions;
using MMU.BoerseDownloader.WpfUI.Models;
using MMU.BoerseDownloader.WpfUI.Services;
using MMU.BoerseDownloader.WpfUI.ViewModels.Shell;

namespace MMU.BoerseDownloader.WpfUI.ViewModels
{
    public class DownloadEntriesOverviewViewModel : ViewModelBase
    {
        private readonly IExceptionHandler _exceptionHandler;
        private readonly IInformationHandler _informationHandler;
        private readonly DownloadEntriesOverviewService _service;
        private IEnumerable<DownloadEntryWithConfiguration> _allDownloadEntries;
        private bool _initialLoaded = false;
        private bool _loading;
        private IEnumerable<DownloadEntryWithConfiguration> _selectedDownloadEntries;
        private bool _showVisisitedDownloadEntries;

        public DownloadEntriesOverviewViewModel(IExceptionHandler exceptionHandler, IInformationHandler informationHandler, DownloadEntriesOverviewService service)
        {
            _exceptionHandler = exceptionHandler;
            _informationHandler = informationHandler;
            _service = service;

            LoadAllEntriesAsync();
        }

        public ICommand CopySelectedUrlsCommand
        {
            get
            {
                return new RelayCommand(() =>
                    {
                        _exceptionHandler.HandledAction(() =>
                        {
                            _service.CopySelectedUrlsToClipBoard(_selectedDownloadEntries);
                        });
                    },
                    canExecute: () => _selectedDownloadEntries.Any());
            }
        }

        public override string DisplayName => "Download-Activity Overview";

        public ObservableCollection<DownloadEntryWithConfiguration> DownloadEntries { get; private set; }

        public ViewModelCommand ReloadAllDownloadEntries
        {
            get
            {
                return new ViewModelCommand("Reload", new RelayCommand(() =>
                {
                    _exceptionHandler.HandledAction(LoadAllEntriesAsync);
                }, () => !_loading));
            }
        }

        public ICommand ResultsSelectionChangedCommand
        {
            get
            {
                return new ParametredRelayCommand(f =>
                {
                    var col = ((IList)f).Cast<DownloadEntryWithConfiguration>().ToList();
                    _selectedDownloadEntries = col;
                });
            }
        }

        public bool ShowVisitedDownloadEntries
        {
            get
            {
                return _showVisisitedDownloadEntries;
            }
            set
            {
                _showVisisitedDownloadEntries = value;
                ApplySearchFilter();
            }
        }

        public ICommand SwitchSelectedLinksVisitedStatus
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _exceptionHandler.HandledAction(() =>
                    {
                        _service.SwitchSelectedLinksVisitedStatus(_selectedDownloadEntries);
                    });
                }, canExecute: () => _selectedDownloadEntries.Any());
            }
        }

        private void ApplySearchFilter()
        {
            var filteredDownloadEntries = _allDownloadEntries;
            if (!_showVisisitedDownloadEntries)
            {
                filteredDownloadEntries = filteredDownloadEntries.Where(f => !f.DownloadLinkIsVisited);
            }

            DownloadEntries = new ObservableCollection<DownloadEntryWithConfiguration>(filteredDownloadEntries);

            if (!_initialLoaded)
            {
                _initialLoaded = true;
                DownloadEntries = new ObservableCollection<DownloadEntryWithConfiguration>(filteredDownloadEntries);
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // UI looks smoother if we operate directly on the List
                    var removedEntries = DownloadEntries.Where(p => filteredDownloadEntries.All(f => f != p)).ToList();
                    removedEntries.ForEach(f => DownloadEntries.Remove(f));

                    var addedEntries = filteredDownloadEntries.Where(p => DownloadEntries.All(f => f != p)).ToList();
                    addedEntries.ForEach(f => DownloadEntries.Add(f));
                });
            }
        }

        private void DownloadEntry_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DownloadEntryWithConfiguration.DownloadLinkIsVisited))
            {
                var obj = (DownloadEntryWithConfiguration)sender;
                if (obj.DownloadLinkIsVisited && !_showVisisitedDownloadEntries)
                {
                    DownloadEntries.Remove(obj);
                }
            }
        }

        private void LoadAllEntriesAsync()
        {
            Task.Run(() =>
            {
                _exceptionHandler.HandledAction(async () =>
                    {
                        _loading = true;
                        await LoadMergeAllEntriesASync();
                    },
                    finallyAction: () =>
                    {
                        _loading = false;
                    });
            });
        }

        private async Task LoadMergeAllEntriesASync()
        {
            var entriesWithConfig = await _service.GetAllEntriesASync();
            entriesWithConfig.ForEach(f => f.PropertyChanged += DownloadEntry_PropertyChanged);
            _allDownloadEntries = entriesWithConfig.ToList();
            ApplySearchFilter();
        }
    }
}