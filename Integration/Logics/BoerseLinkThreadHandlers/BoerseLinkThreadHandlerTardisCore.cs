using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using MMU.BoerseDownloader.Common.Infrastructure;
using MMU.BoerseDownloader.Integration.Infrastructure;
using MMU.BoerseDownloader.Integration.Infrastructure.Extensions;
using MMU.BoerseDownloader.Integration.Logics.Factories;
using MMU.BoerseDownloader.Model;
using MMU.BoerseDownloader.Model.Enumerations;

namespace MMU.BoerseDownloader.Integration.Logics.BoerseLinkThreadHandlers
{
    public class BoerseLinkThreadHandlerTardisCore : BoerseLinkThreadHandler
    {
        public BoerseLinkThreadHandlerTardisCore(WebBrowserAdapterFactory webBrowserAdapterFactory) : base(webBrowserAdapterFactory)
        {
        }

        internal override BoerseLinkProvider BoerseLinkProvider
        {
            get
            {
                return BoerseLinkProvider.TardisCore;
            }
        }

        private string GetDownloadEntryName(HtmlNode singleSpan)
        {
            var foundBrElement = false;
            HtmlNode breakElement = null;
            HtmlNode parentSpan = singleSpan;

            while (!foundBrElement)
            {
                parentSpan = parentSpan.NavigateToElementOfType("span", Model.Enumerations.HtmlNavigationType.Parent);
                breakElement = parentSpan.NavigateToElementOfType("br", Model.Enumerations.HtmlNavigationType.PreviousSibling);
                foundBrElement = breakElement != null;
            }

            var textElement = breakElement.NavigateToElementOfType("#text", Model.Enumerations.HtmlNavigationType.PreviousSibling);
            var result = textElement.InnerText;

            if (result.StartsWith("\n"))
            {
                result = result.Substring(1);
            }

            return result;
        }

        private string GetExternalUrl(HtmlNode singleSpan)
        {
            var containerSpan = singleSpan.NavigateToElementOfType("span", Model.Enumerations.HtmlNavigationType.Parent).NavigateToElementOfType("span", Model.Enumerations.HtmlNavigationType.Parent);
            var links = containerSpan.Descendants("a");
            var uploadedLink = links.FirstOrDefault(f => f.InnerText == "Uploaded.net");

            if (uploadedLink != null)
            {
                return uploadedLink.Attributes["href"].Value;
            }

            return string.Empty;
        }

        private IEnumerable<DownloadEntry> ReadActualEpisodes(HtmlNode threadHtmlNode, DownloadContext downloadContext)
        {
            var result = new List<DownloadEntry>();
            var singleEpisodeSpans = threadHtmlNode.Descendants().Where(f => f.Name == "span" && f.InnerText == "Aktuelle Folge");

            foreach (var singleSpan in singleEpisodeSpans)
            {
                var externalLink = GetExternalUrl(singleSpan);
                var entryName = GetDownloadEntryName(singleSpan);
                var lastThreadUpdate = GetLastThreadUpdate(threadHtmlNode);

                var downloadEntry = new DownloadEntry(downloadContext.Name, externalLink, entryName, lastThreadUpdate, false);
                result.Add(downloadEntry);
            }

            return result;
        }

        private bool TryReadingSeasonPack(HtmlNode threadHtmlNode, DownloadContext downloadContext, out DownloadEntry seasonPackEntry)
        {
            var seasonPackSpan = threadHtmlNode.Descendants("span").FirstOrDefault(f => f.InnerText == "Staffel");
            if (seasonPackSpan != null)
            {
                var parentSpan = seasonPackSpan.NavigateToElementOfType("span", Model.Enumerations.HtmlNavigationType.Parent);
                if (parentSpan != null)
                {
                    var uploadedLink = parentSpan.NavigateToElement(f => f.Name == "a" && f.InnerText.ContainsCaseInsensitive("Uploaded"), Model.Enumerations.HtmlNavigationType.NextSibling);
                    if (uploadedLink != null)
                    {
                        var link = uploadedLink.Attributes["href"].Value;
                        seasonPackEntry = new DownloadEntry(downloadContext.Name, link, Constants.DOWNLOADENTRYTITLE_SEASONPACK, GetLastThreadUpdate(threadHtmlNode), true);
                        return true;
                    }
                }
            }

            seasonPackEntry = null;
            return false;
        }

        protected override IReadOnlyCollection<DownloadEntry> ReadAllDownloadEntriesFromThread(HtmlNode threadHtmlNode, DownloadContext downloadContext)
        {
            var result = new List<DownloadEntry>();

            var actualEpisodes = ReadActualEpisodes(threadHtmlNode, downloadContext);
            result.AddRange(actualEpisodes);

            DownloadEntry seasonPack;
            if (TryReadingSeasonPack(threadHtmlNode, downloadContext, out seasonPack))
            {
                result.Add(seasonPack);
            }

            return result;
        }
    }
}