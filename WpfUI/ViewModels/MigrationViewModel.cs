using System.Collections.ObjectModel;
using MMU.BoerseDownloader.WpfUI.Commands;
using MMU.BoerseDownloader.WpfUI.Handlers.ExceptionHandling;
using MMU.BoerseDownloader.WpfUI.Models;
using MMU.BoerseDownloader.WpfUI.Services;
using MMU.BoerseDownloader.WpfUI.ViewModels.Shell;

namespace MMU.BoerseDownloader.WpfUI.ViewModels
{
    public class MigrationViewModel : ViewModelBase
    {
        private readonly IExceptionHandler _exceptionHandler;
        private readonly MigrationService _service;
        private bool _migrationProcessing;

        public MigrationViewModel(IExceptionHandler exceptionHandler, MigrationService service)
        {
            _exceptionHandler = exceptionHandler;
            _service = service;
            _service.Initialize(OnNewProgressInformation);
        }

        public override string DisplayName { get; } = "Migration from File to DB";

        public ObservableCollection<StringValue> MigrationProcessEntries { get; } = new ObservableCollection<StringValue>();

        public ViewModelCommand StartingMigratingDownloadEntryIdentifiersVmc
        {
            get
            {
                return new ViewModelCommand("Identifiers", new RelayCommand(() =>
                {
                    _exceptionHandler.HandledActionAsync(async () =>
                        {
                            _migrationProcessing = true;
                            ClearMigrationProcessEntries();
                            await _service.MigrateDownloadEntryIdentifiersAsync();
                        },
                        finallyAction: () => _migrationProcessing = false);
                }, canExecute: () => !_migrationProcessing));
            }
        }

        public ViewModelCommand StartMigratingBoerseUserVmc
        {
            get
            {
                return new ViewModelCommand("User", new RelayCommand(() =>
                    {
                        _exceptionHandler.HandledActionAsync(async () =>
                            {
                                _migrationProcessing = true;
                                ClearMigrationProcessEntries();
                                await _service.MigrateBoerseUserAsync();
                            },
                            finallyAction: () => _migrationProcessing = false);
                    },
                    canExecute: () => !_migrationProcessing));
            }
        }

        public ViewModelCommand StartMigratingDownloadContextsVmc
        {
            get
            {
                return new ViewModelCommand("Contexts", new RelayCommand(() =>
                    {
                        _exceptionHandler.HandledActionAsync(async () =>
                            {
                                _migrationProcessing = true;
                                ClearMigrationProcessEntries();
                                await _service.MigrateContextsAsync();
                            },
                            finallyAction: () => _migrationProcessing = false);
                    },
                    canExecute: () => !_migrationProcessing));
            }
        }

        public ViewModelCommand StartMigrationgDownloadEntryConfigurationsVmc
        {
            get
            {
                return new ViewModelCommand("Entries", new RelayCommand(() =>
                    {
                        _exceptionHandler.HandledActionAsync(async () =>
                            {
                                _migrationProcessing = true;
                                ClearMigrationProcessEntries();
                                await _service.MigrateDownloadEntryConfigurationsAsync();
                            },
                            finallyAction: () => _migrationProcessing = false);
                    },
                    canExecute: () => !_migrationProcessing));
            }
        }

        public ViewModelCommand StopMigrationVmc
        {
            get
            {
                return new ViewModelCommand("Stop", new RelayCommand(() =>
                    {
                        _service.StopMigration();
                        _migrationProcessing = false;
                    },
                    canExecute: () => _migrationProcessing));
            }
        }

        private void ClearMigrationProcessEntries()
        {
            MigrationProcessEntries.Clear();
        }

        private void OnNewProgressInformation(string entry)
        {
            MigrationProcessEntries.Add(new StringValue(entry));
        }
    }
}