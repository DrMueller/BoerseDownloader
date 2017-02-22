using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using MMU.BoerseDownloader.Common.Infrastructure;
using MMU.BoerseDownloader.Integration.Infrastructure;
using MMU.BoerseDownloader.Integration.Infrastructure.Extensions;
using MMU.BoerseDownloader.Integration.Logics.Factories;
using MMU.BoerseDownloader.Integration.Model.Enumerations;
using MMU.BoerseDownloader.Model;
using MMU.BoerseDownloader.Model.Enumerations;

namespace MMU.BoerseDownloader.Integration.Logics.BoerseLinkThreadHandlers
{
    public class BoerseLinkThreadHandlerBauklo : BoerseLinkThreadHandler
    {
        public BoerseLinkThreadHandlerBauklo(WebBrowserAdapterFactory webBrowserAdapterFactory) : base(webBrowserAdapterFactory)
        {
        }

        internal override BoerseLinkProvider BoerseLinkProvider { get; } = BoerseLinkProvider.Bauklo;

        private static string GetTitleFromLink(HtmlNode linkNode)
        {
            var breakElement = linkNode.NavigateToElementOfType("b", HtmlNavigationType.Parent);
            do
            {
                var spans = breakElement.Descendants("span");
                foreach (var sp in spans)
                {
                    var styleAttr = sp.Attributes["style"];
                    if (styleAttr != null && styleAttr.Value.Contains("rgb(255, 0, 0);"))
                    {
                        var title = sp.InnerText;
                        return title;
                    }
                }

                breakElement = breakElement.NavigateToElementOfType("b", HtmlNavigationType.PreviousSibling);
            }
            while (breakElement != null);

            return string.Empty;
        }

        private IEnumerable<DownloadEntry> ReadActualEpisodes(HtmlNode threadHtmlNode, DownloadContext downloadContext)
        {
            var result = new List<DownloadEntry>();

            var allLinkElements = threadHtmlNode.Descendants("a").Where(f =>
            {
                var attr = f.Attributes["href"].Value;
                return attr.ContainsCaseInsensitive("ncrypt.in");
            });

            foreach (var linkElement in allLinkElements)
            {
                var title = GetTitleFromLink(linkElement);

                // Assuming it is not Season-Pack
                if (!string.IsNullOrEmpty(title))
                {
                    var link = linkElement.Attributes["href"].Value;
                    var downloadEntry = new DownloadEntry(downloadContext.Name, link, title, GetLastThreadUpdate(threadHtmlNode), false);
                    result.Add(downloadEntry);
                }
            }

            return result;
        }

        private bool TryReadingSeasonPack(HtmlNode threadHtmlNode, DownloadContext downloadContext, out DownloadEntry seasonPackEntry)
        {
            var staffelpackElement = threadHtmlNode.Descendants("b").FirstOrDefault(f => f.InnerText.Contains("Staffelpack"));
            if (staffelpackElement != null)
            {
                var parentSpan = staffelpackElement.NavigateToElementOfType("span", Model.Enumerations.HtmlNavigationType.Parent);
                var nextBlock = parentSpan.NavigateToElementOfType("b", Model.Enumerations.HtmlNavigationType.NextSibling);

                var linkElement = nextBlock.Descendants("a").First();

                var link = linkElement.Attributes["href"].Value;
                seasonPackEntry = new DownloadEntry(downloadContext.Name, link, Constants.DOWNLOADENTRYTITLE_SEASONPACK, GetLastThreadUpdate(threadHtmlNode), true);
                return true;
            }

            seasonPackEntry = null;
            return false;
        }

        protected override IReadOnlyCollection<DownloadEntry> ReadAllDownloadEntriesFromThread(HtmlNode threadHtmlNode, DownloadContext downloadContext)
        {
            var result = new List<DownloadEntry>();
            DownloadEntry seasonPack;
            if (TryReadingSeasonPack(threadHtmlNode, downloadContext, out seasonPack))
            {
                result.Add(seasonPack);
            }

            var actualEpisodes = ReadActualEpisodes(threadHtmlNode, downloadContext);
            foreach (var actualEpisode in actualEpisodes)
            {
                if (seasonPack == null || actualEpisode.DownloadLink != seasonPack.DownloadLink)
                {
                    result.Add(actualEpisode);
                }
            }

            return result.AsReadOnly();
        }
    }
}