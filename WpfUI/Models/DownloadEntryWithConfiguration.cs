using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MMU.BoerseDownloader.WpfUI.Models
{
    public class DownloadEntryWithConfiguration : INotifyPropertyChanged
    {
        private bool _downloadLinkIsVisited;

        public DownloadEntryWithConfiguration(long downloadEntryId, string downloadContextName, bool isSeasonPackEntry, bool downloadEntryHasMultipleTitles, string downloadLink, Model.DownloadEntryTitleCollection downloadEntryTitles, DateTime? lastThreadUpdate, bool downloadLinkIsVisited, bool isInvalidEntry, string downloadLinkIdentifier)
        {
            DownloadEntryId = downloadEntryId;
            DownloadEntryTitles = downloadEntryTitles;
            DownloadContextName = downloadContextName;
            IsSeasonPackEntry = isSeasonPackEntry;
            DownloadEntryHasMultipleTitles = downloadEntryHasMultipleTitles;
            DownloadLink = downloadLink;
            LastThreadUpdate = lastThreadUpdate;
            _downloadLinkIsVisited = downloadLinkIsVisited;
            IsInvalidEntry = isInvalidEntry;
            DownloadLinkIdentifier = downloadLinkIdentifier;
        }

        public string DownloadContextName { get; }

        public bool DownloadEntryHasMultipleTitles { get; private set; }

        public long DownloadEntryId { get; set; }

        // Used to show in UI
        public string DownloadEntryTitleFormatted
        {
            get
            {
                var titlesString = DownloadEntryTitles.Select(f => f.Title);
                var result = string.Join(Environment.NewLine, titlesString);
                return result;
            }
        }

        public Model.DownloadEntryTitleCollection DownloadEntryTitles { get; private set; }

        public string DownloadEntryTitleShort
        {
            get
            {
                string result;
                if (IsSeasonPackEntry)
                {
                    const string SEASONPACK_SHORTNAME = "Seasonpack";
                    result = string.Concat(SEASONPACK_SHORTNAME, "_", DownloadContextName);
                }
                else
                {
                    result = DownloadEntryTitles.ShortName;
                }

                return result;
            }
        }

        public string DownloadLink { get; private set; }

        public string DownloadLinkIdentifier { get; set; }

        public bool DownloadLinkIsVisited
        {
            get
            {
                return _downloadLinkIsVisited;
            }
            set
            {
                if (value != _downloadLinkIsVisited)
                {
                    _downloadLinkIsVisited = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsInvalidEntry { get; private set; }

        public bool IsSeasonPackEntry { get; }

        public DateTime? LastThreadUpdate { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return DownloadEntryTitleFormatted;
        }

        private void OnPropertyChanged([CallerMemberName] string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}