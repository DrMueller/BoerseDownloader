using System.Web;

namespace MMU.BoerseDownloader.Model
{
    public class DownloadEntryTitle
    {
        public DownloadEntryTitle(string str)
        {
            Title = HttpUtility.HtmlDecode(str);
        }

        public string Title { get; }
    }
}