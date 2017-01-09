using System;
using System.Diagnostics;

namespace MMU.BoerseDownloader.Logics.Logics
{
    public class ProcessingTimer : IDisposable
    {
        private const Double TIMER_INTERVAL = 1000;
        private readonly Stopwatch _stopwatch;
        private readonly System.Timers.Timer _timer;
        private bool _disposed;
        private IProgress<string> _progress;

        public ProcessingTimer()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
            _timer = new System.Timers.Timer(TIMER_INTERVAL);
            _timer.Elapsed += Timer_Elapsed;
        }

        private string ElapsedTimeDescription
        {
            get
            {
                var result = _stopwatch.Elapsed.ToString(@"mm\:ss");
                return result;
            }
        }

        internal void Initialize(IProgress<string> progress)
        {
            _progress = progress;
        }

        internal void StartShowingProcess()
        {
            _stopwatch.Reset();
            _timer.Start();
            _stopwatch.Start();
        }

        internal void WrapUpAndFinishProcess()
        {
            ReportInformation($"Finished Loading. Elapsed: {ElapsedTimeDescription}.");
            _stopwatch.Stop();
            _timer.Stop();
        }

        private void ReportInformation(string text)
        {
            _progress.Report(text);
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var str = $"Processing Entries. Time elapsed: {ElapsedTimeDescription}.";
            ReportInformation(str);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _timer.Dispose();
                }

                //// Unmanaged resources are released here.
                _disposed = true;
            }
        }

        ~ProcessingTimer()
        {
            Dispose(false);
        }
    }
}