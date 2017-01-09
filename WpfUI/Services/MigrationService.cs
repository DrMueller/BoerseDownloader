using System;
using System.Threading;
using System.Threading.Tasks;
using MMU.BoerseDownloader.Logics;

namespace MMU.BoerseDownloader.WpfUI.Services
{
    public class MigrationService : IDisposable
    {
        private readonly MigrationLogic _logic;
        private readonly Progress<string> _progress = new Progress<string>();
        private CancellationTokenSource _cancellationTokenSource;
        private bool _disposed;
        private Action<string> _newProgressInformationCallback;

        public MigrationService(MigrationLogic logic)
        {
            _logic = logic;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal void Initialize(Action<string> newProgressInformationCallback)
        {
            _newProgressInformationCallback = newProgressInformationCallback;
            _logic.Initialize(_progress);
            _progress.ProgressChanged += Progress_ProgressChanged;
        }

        internal async Task MigrateBoerseUserAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await _logic.MigrateBoerseUserAsync(_cancellationTokenSource.Token);
        }

        internal async Task MigrateContextsAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await _logic.MigrateContextsAsync(_cancellationTokenSource.Token);
        }

        internal async Task MigrateDownloadEntryConfigurationsAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await _logic.MigrateDownloadEntryConfigurationsAsync(_cancellationTokenSource.Token);
        }

        internal async Task MigrateDownloadEntryIdentifiersAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await _logic.MigrateDownloadEntryIdentifiers(_cancellationTokenSource.Token);
        }

        internal void StopMigration()
        {
            _cancellationTokenSource.Cancel();
        }

        private void Progress_ProgressChanged(object sender, string e)
        {
            _newProgressInformationCallback(e);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _cancellationTokenSource.Dispose();
                }

                //// Unmanaged resources are released here.
                _disposed = true;
            }
        }

        ~MigrationService()
        {
            Dispose(false);
        }
    }
}