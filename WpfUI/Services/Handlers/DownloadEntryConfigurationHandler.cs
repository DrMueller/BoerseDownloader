using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MMU.BoerseDownloader.Logics;
using MMU.BoerseDownloader.Model;
using MMU.BoerseDownloader.WpfUI.Models;

namespace MMU.BoerseDownloader.WpfUI.Services.Handlers
{
    public class DownloadEntryConfigurationHandler
    {
        private readonly DownloadEntryConfigurationLogic _configurationLogic;

        public DownloadEntryConfigurationHandler(DownloadEntryConfigurationLogic configurationLogic)
        {
            _configurationLogic = configurationLogic;
        }

        public IReadOnlyCollection<DownloadEntryWithConfiguration> EnrichDownloadEntries(IEnumerable<DownloadEntry> downloadEntries)
        {
            var allConfigEntries = _configurationLogic.GetAllConfigurationEntries();
            var result = downloadEntries.Select(f =>
            {
                bool isVisited = false;
                long configEntryId = 0;
                DownloadEntryConfiguration configEntry;
                if (TryGettingPersistedConfigurationEntry(allConfigEntries, f, out configEntry))
                {
                    isVisited = configEntry.IsLinkVisited;
                    configEntryId = configEntry.Id;
                }

                var entry = new DownloadEntryWithConfiguration(
                    configEntryId,
                    f.DownloadContextName,
                    f.IsSeasonPackEntry,
                    f.DownloadEntryTitles.Count > 1,
                    f.DownloadLink,
                    f.DownloadEntryTitles,
                    f.LastThreadUpdate,
                    isVisited,
                    f.IsInvalidEntry,
                    f.DownloadLinkIdentifier);

                entry.PropertyChanged += DownloadEntry_PropertyChanged;
                return entry;
            });

            return result.ToList();
        }

        private void DownloadEntry_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DownloadEntryWithConfiguration.DownloadLinkIsVisited))
            {
                var obj = (DownloadEntryWithConfiguration)sender;
                var downloadEntryConfig = new DownloadEntryConfiguration
                {
                    Id = obj.DownloadEntryId,
                    DownloadLinkIdentifier = obj.DownloadLinkIdentifier,
                    Title = obj.DownloadEntryTitleShort,
                    IsLinkVisited = obj.DownloadLinkIsVisited
                };
                _configurationLogic.SaveDownloadEntryConfiguration(downloadEntryConfig);
                obj.DownloadEntryId = downloadEntryConfig.Id;
            }
        }

        private bool TryGettingPersistedConfigurationEntry(IReadOnlyCollection<DownloadEntryConfiguration> persistedConfigurationEntries, DownloadEntry downloadEntry, out DownloadEntryConfiguration configEntry)
        {
            // We pririorize the LinkIdentifier, but use the ShortTitle for the backword-compatibility
            configEntry = persistedConfigurationEntries.FirstOrDefault(f => f.DownloadLinkIdentifier == downloadEntry.DownloadLinkIdentifier);
            if (configEntry == null)
            {
                configEntry = persistedConfigurationEntries.FirstOrDefault(f => f.Title == downloadEntry.DownloadEntryTitleShort);
            }

            return configEntry != null;
        }
    }
}