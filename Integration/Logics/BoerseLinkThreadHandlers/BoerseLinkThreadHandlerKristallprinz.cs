using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using MMU.BoerseDownloader.Integration.Infrastructure;
using MMU.BoerseDownloader.Integration.Infrastructure.Extensions;
using MMU.BoerseDownloader.Integration.Logics.Factories;
using MMU.BoerseDownloader.Model;
using MMU.BoerseDownloader.Model.Enumerations;

namespace MMU.BoerseDownloader.Integration.Logics.BoerseLinkThreadHandlers
{
    public class BoerseLinkThreadHandlerKristallprinz : BoerseLinkThreadHandler
    {
        public BoerseLinkThreadHandlerKristallprinz(WebBrowserAdapterFactory webBrowserAdapterFactory) : base(webBrowserAdapterFactory)
        {
        }

        internal override BoerseLinkProvider BoerseLinkProvider
        {
            get
            {
                return BoerseLinkProvider.Kristallprinz;
            }
        }

        private static DownloadEntryTitleCollection GetDownloadEntryTitles(HtmlNode actualEpisodesSpanElement)
        {
            var titlesOfActualEpisodes = new List<DownloadEntryTitle>();
            var moving = actualEpisodesSpanElement.ParentNode.NextSibling;

            while (moving.Name != "div")
            {
                var innerSpanText = moving.InnerText;

                var separatedTttles = innerSpanText.Split('\n');
                foreach (var title in separatedTttles)
                {
                    if (title.CheckIfValidDownloadEntryTitle())
                    {
                        titlesOfActualEpisodes.Add(new DownloadEntryTitle(title));
                    }
                }

                moving = moving.NextSibling;
            }

            var result = new DownloadEntryTitleCollection(titlesOfActualEpisodes);
            return result;
        }

        private static string GetExternalLinkUrl(HtmlNode actualEpisodesSpanElement)
        {
            var parentBlockNode = actualEpisodesSpanElement.ParentNode;
            var nextDivNode = parentBlockNode.GetNextElementOfType("div", Model.Enumerations.HtmlNavigationType.NextSibling);
            var urlNode = nextDivNode.Descendants("a").First();

            var result = urlNode.Attributes.First(f => f.Name == "href").Value;
            return result;
        }

        private IEnumerable<DownloadEntry> ReadActualEpisodes(HtmlNode threadHtmlNode, DownloadContext downloadContext)
        {
            var result = new List<DownloadEntry>();
            var actualEpisodesSpanElement = threadHtmlNode.Descendants("span").FirstOrDefault(f => f.InnerText.Contains("Aktuelle Episode(n)"));

            if (actualEpisodesSpanElement != null)
            {
                var downloadLink = GetExternalLinkUrl(actualEpisodesSpanElement);
                var entryTitles = GetDownloadEntryTitles(actualEpisodesSpanElement);

                var lastUploadDate = GetLastThreadUpdate(threadHtmlNode);
                var newEntry = new DownloadEntry(downloadContext.Name, downloadLink, entryTitles, lastUploadDate, false);
                result.Add(newEntry);
            }

            return result;
        }

        private bool TryReadingSeasonPack(HtmlNode threadHtmlNode, DownloadContext downloadContext, out DownloadEntry seasonPackEntry)
        {
            var seasonPackSpan = threadHtmlNode.Descendants("span").FirstOrDefault(f => f.InnerText.Contains("Staffelpack"));
            if (seasonPackSpan != null)
            {
                var downloadLink = GetExternalLinkUrl(seasonPackSpan);
                var entryTitle = Constants.DOWNLOADENTRYTITLE_SEASONPACK;
                var lastUploadDate = GetLastThreadUpdate(threadHtmlNode);
                seasonPackEntry = new DownloadEntry(downloadContext.Name, downloadLink, entryTitle, lastUploadDate, true);
                return true;
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

            return result.AsReadOnly();
        }
    }
}