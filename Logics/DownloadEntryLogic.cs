using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MMU.BoerseDownloader.Integration;
using MMU.BoerseDownloader.Logics.Logics;
using MMU.BoerseDownloader.Model;

namespace MMU.BoerseDownloader.Logics
{
    public class DownloadEntryLogic
    {
        private readonly BoerseUserConfigurationLogic _boerseUserConfigurationLogic;
        private readonly DownloadContextConfigurationLogic _downloadContextConfigurationLogic;
        private readonly DownloadEntryEqualityComparer _downloadEntryEqualityComparer;
        private readonly BoerseHtmlIntegrationFactory _integrationFactory;
        private readonly ProcessingTimer _processingTimer;
        private IProgress<string> _progress;

        public DownloadEntryLogic(BoerseHtmlIntegrationFactory integrationFactory, DownloadContextConfigurationLogic downloadContextConfigurationLogic, BoerseUserConfigurationLogic boerseUserConfigurationLogic, DownloadEntryEqualityComparer downloadEntryEqualityComparer, ProcessingTimer processingTimer)
        {
            _integrationFactory = integrationFactory;
            _downloadContextConfigurationLogic = downloadContextConfigurationLogic;
            _boerseUserConfigurationLogic = boerseUserConfigurationLogic;
            _downloadEntryEqualityComparer = downloadEntryEqualityComparer;
            _processingTimer = processingTimer;
        }

        public async Task<IReadOnlyCollection<DownloadEntry>> GetallEntriesAsync()
        {
            ReportInformation("Loading BoerseUser");
            BoerseUser boerseUser;

            if (!TryLoadingBoerseUser(out boerseUser))
            {
                ReportInformation("Boerse-User is not configured! Please switch the the 'Config User' View.");
                return Enumerable.Empty<DownloadEntry>().ToList().AsReadOnly();
            }

            ReportInformation("Loading Configurations...");
            var concurrentDownloadEntriesFromWebSite = new System.Collections.Concurrent.ConcurrentBag<DownloadEntry>();
            List<DownloadEntry> allDownloadEntries = new List<DownloadEntry>();
            _processingTimer.StartShowingProcess();

            await Task.Run(() =>
            {
                var allDownloadContexts = _downloadContextConfigurationLogic.LoadAll();

                Parallel.ForEach(allDownloadContexts, context =>
                {
                    var downloadEntriesFromBoerseWebsite = DownloadEntries(context, boerseUser);
                    foreach (var de in downloadEntriesFromBoerseWebsite)
                    {
                        concurrentDownloadEntriesFromWebSite.Add(de);
                    }
                });

                allDownloadEntries = concurrentDownloadEntriesFromWebSite.Distinct(_downloadEntryEqualityComparer).ToList();
                _processingTimer.WrapUpAndFinishProcess();
            });

            return allDownloadEntries.AsReadOnly();
        }

        public void Initialize(IProgress<string> progress)
        {
            _progress = progress;
            _processingTimer.Initialize(progress);
        }

        private IEnumerable<DownloadEntry> DownloadEntries(DownloadContext downloadContext, BoerseUser boerseUser)
        {
            IEnumerable<DownloadEntry> result;

            try
            {
                var integration = _integrationFactory.Create();
                result = integration.Download(downloadContext, boerseUser);
            }
            catch (Exception ex)
            {
                result = new List<DownloadEntry>
                {
                    new DownloadEntry(downloadContext.Name, ex)
                };
            }

            return result;
        }

        private void ReportInformation(string text)
        {
            _progress.Report(text);
        }

        private bool TryLoadingBoerseUser(out BoerseUser boerseUser)
        {
            if (!_boerseUserConfigurationLogic.UserIsConfigured)
            {
                boerseUser = null;
                return false;
            }

            boerseUser = _boerseUserConfigurationLogic.Load();
            return true;
        }
    }
}