using MMU.BoerseDownloader.Model.Interfaces;

namespace MMU.BoerseDownloader.Model
{
    public class DownloadEntryConfiguration : IIdentifiable
    {
        public string DownloadLinkIdentifier { get; set; } // http://filecrypt.cc/Container/5599B13C05.html

        public bool IsLinkVisited { get; set; }

        public string Title { get; set; }

        public long Id { get; set; }
    }

    public class DownloadEntryConfigurationLegacy
    {
        public string DownloadEntryTitle { get; set; }

        public bool DownloadLinkIsVisited { get; set; }
    }
}