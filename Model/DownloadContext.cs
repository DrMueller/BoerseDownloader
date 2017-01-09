using MMU.BoerseDownloader.Model.Enumerations;

namespace MMU.BoerseDownloader.Model
{
    public class DownloadContext : Interfaces.IIdentifiable
    {
        public BoerseLinkProvider BoerseLinkProvider { get; set; }

        public string Name { get; set; }

        public string ThreadUrl { get; set; }

        public long Id { get; set; }
    }
}