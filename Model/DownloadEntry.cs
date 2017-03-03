using System;
using System.Collections.Generic;
using System.Linq;

namespace MMU.BoerseDownloader.Model
{
    public class DownloadEntry
    {
        public DownloadEntry(string downloadContextName, string downloadLink, DownloadEntryTitleCollection downloadEntryTitles, DateTime? lastThreadUpdate, bool isSeasonPackEntry)
        {
            DownloadContextName = downloadContextName;
            DownloadLink = downloadLink;
            DownloadEntryTitles = downloadEntryTitles;
            LastThreadUpdate = lastThreadUpdate;
            IsSeasonPackEntry = isSeasonPackEntry;

            CalculateDownloadLinkIdentifier();
        }

        public DownloadEntry(string downloadContextName, string downloadLink, string downloadEntryTitle, DateTime? lastThreadUpdate, bool isSeasonPackEntry) :
            this(downloadContextName, downloadLink, new DownloadEntryTitleCollection(new List<DownloadEntryTitle> { new DownloadEntryTitle(downloadEntryTitle) }), lastThreadUpdate, isSeasonPackEntry)
        {
        }

        public DownloadEntry(string downloadContextName, Exception ex) : this(downloadContextName, string.Empty, ex.Message, null, false)
        {
            IsInvalidEntry = true;
        }

        public string DownloadContextName { get; }

        public DownloadEntryTitleCollection DownloadEntryTitles { get; }

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

        public string DownloadLink { get; }

        public string DownloadLinkIdentifier { get; private set; } // http://filecrypt.cc/Container/5599B13C05.html

        public bool IsInvalidEntry { get; private set; }

        public bool IsSeasonPackEntry { get; }

        public DateTime? LastThreadUpdate { get; private set; }

        public override bool Equals(object obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            var str = $"{DownloadContextName}_{DownloadEntryTitles}_{DownloadLink}";
            return str.GetHashCode();
        }

        public override string ToString()
        {
            return DownloadEntryTitles.ToString();
        }

        private void CalculateDownloadLinkIdentifier()
        {
            if (string.IsNullOrEmpty(DownloadLink))
            {
                DownloadLinkIdentifier = string.Empty;
                return;
            }

            var splittedByPath = DownloadLink.Split(new[] { @"/" }, StringSplitOptions.RemoveEmptyEntries);
            var linkIdentifier = splittedByPath.Last();

            DownloadLinkIdentifier = linkIdentifier;
        }
    }
}