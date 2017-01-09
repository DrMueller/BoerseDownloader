using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.ObjectBuilder2;
using MMU.BoerseDownloader.Logics;
using MMU.BoerseDownloader.WpfUI.Handlers.InformationHandling;
using MMU.BoerseDownloader.WpfUI.Infrastructure;
using MMU.BoerseDownloader.WpfUI.Models;
using MMU.BoerseDownloader.WpfUI.Services.Handlers;

namespace MMU.BoerseDownloader.WpfUI.Services
{
    public class DownloadEntriesOverviewService
    {
        private readonly DownloadEntryConfigurationHandler _configurationHandler;
        private readonly IInformationHandler _informationHandler;
        private readonly DownloadEntryLogic _logic;
        private readonly Progress<string> _progress = new Progress<string>();

        public DownloadEntriesOverviewService(IInformationHandler informationHandler, DownloadEntryLogic logic, DownloadEntryConfigurationHandler configurationHandler)
        {
            _informationHandler = informationHandler;
            _logic = logic;
            _progress.ProgressChanged += Progress_ProgressChanged;
            _logic.Initialize(_progress);
            _configurationHandler = configurationHandler;
        }

        internal void CopySelectedUrlsToClipBoard(IEnumerable<DownloadEntryWithConfiguration> selectedDownloadEntries)
        {
            var selectedUrls = selectedDownloadEntries.Select(f => f.DownloadLink);
            var sb = new StringBuilder();

            selectedUrls.ForEach(f =>
            {
                sb.AppendLine(f);
            });

            var str = sb.ToString();
            Clipboard.SetText(str);
        }

        internal async Task<IReadOnlyCollection<DownloadEntryWithConfiguration>> GetAllEntriesASync()
        {
            //new UiBlockDetector();
            var allEntries = await _logic.GetallEntriesAsync();
            var entriesWithConfig = _configurationHandler.EnrichDownloadEntries(allEntries);
            return entriesWithConfig;
        }

        internal void SwitchSelectedLinksVisitedStatus(IEnumerable<DownloadEntryWithConfiguration> selectedDownloadEntries)
        {
            selectedDownloadEntries.ForEach(f => f.DownloadLinkIsVisited = !f.DownloadLinkIsVisited);
        }

        private void Progress_ProgressChanged(object sender, string e)
        {
            _informationHandler.HandleInformation(Models.Enumerations.InformationType.Information, e);
        }
    }
}