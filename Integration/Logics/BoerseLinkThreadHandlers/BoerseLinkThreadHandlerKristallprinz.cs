using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using MMU.BoerseDownloader.Common.Exceptions;
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

        internal override BoerseLinkProvider BoerseLinkProvider => BoerseLinkProvider.Kristallprinz;

        private static DownloadEntryTitleCollection GetDownloadEntryTitles(HtmlNode actualEpisodesSpanElement)
        {
            // We have no clue, how to find the title, so we search for an not-empty, non break block without subelements.
            // If the first element is a span, we navigate to it

            var titlesOfActualEpisodes = new List<DownloadEntryTitle>();

            var titlesContainer = actualEpisodesSpanElement;
            if (titlesContainer.FirstChild.Name == "span")
            {
                titlesContainer = titlesContainer.FirstChild;
            }

            var cnter = 0;
            HtmlNode checkedSubElement;
            do
            {
                checkedSubElement = titlesContainer.ChildNodes[cnter];
                if (checkedSubElement.Name == "b" && checkedSubElement.ChildNodes.All(f => f.Name == "br" || f.Name == "#text"))
                {
                    var innerSpanText = checkedSubElement.InnerText;

                    var separatedTttles = innerSpanText.Split('\n');
                    foreach (var title in separatedTttles)
                    {
                        if (title.CheckIfValidDownloadEntryTitle())
                        {
                            titlesOfActualEpisodes.Add(new DownloadEntryTitle(title));
                        }
                    }
                }

                cnter++;
            }
            while (checkedSubElement.Name != "div" && cnter < titlesContainer.ChildNodes.Count);

            var result = new DownloadEntryTitleCollection(titlesOfActualEpisodes);
            return result;
        }

        private static string GetExternalLinkUrl(HtmlNode actualEpisodesDivElement)
        {
            var nextDivNode = actualEpisodesDivElement.ChildNodes.FirstOrDefault(f => f.Name == "div");
            HtmlNode urlNode;
            if (nextDivNode != null)
            {
                urlNode = nextDivNode.Descendants("a").FirstOrDefault();
            }
            else
            {
                urlNode = actualEpisodesDivElement.Descendants().FirstOrDefault(f => f.Name == "a");
            }

            if (urlNode == null)
            {
                throw new DownloadException("No Link for Uploaded found.");
            }

            var result = urlNode.Attributes.First(f => f.Name == "href").Value;
            return result;
        }

        private IEnumerable<DownloadEntry> ReadActualEpisodes(HtmlNode threadHtmlNode, DownloadContext downloadContext)
        {
            var result = new List<DownloadEntry>();
            HtmlNode actualEpisdesDivElement;

            if (TryReadingActualEpisodesDivElement(threadHtmlNode, out actualEpisdesDivElement))
            {
                var uploadedElements = actualEpisdesDivElement.Descendants("a").Where(f => f.InnerText == "Uploaded");

                foreach (var uploadedEle in uploadedElements)
                {
                    var downloadLink = uploadedEle.Attributes.First(f => f.Name == "href").Value;
                    var parentSpan = uploadedEle.NavigateToElementOfType("span", Model.Enumerations.HtmlNavigationType.Parent);
                    var spanWithTitles = parentSpan.NavigateToElementOfType("span", Model.Enumerations.HtmlNavigationType.PreviousSibling);

                    if (spanWithTitles == null)
                    {
                        spanWithTitles = parentSpan.NavigateToElementOfType("span", Model.Enumerations.HtmlNavigationType.Parent);
                    }

                    var entryTitles = GetDownloadEntryTitles(spanWithTitles);

                    var lastUploadDate = GetLastThreadUpdate(threadHtmlNode);
                    var newEntry = new DownloadEntry(downloadContext.Name, downloadLink, entryTitles, lastUploadDate, false);

                    result.Add(newEntry);
                }
            }
            else
            {
                throw new DownloadException("Could not find an entry-point for the actual episodes");
            }

            return result;
        }

        private bool TryReadingActualEpisodesDivElement(HtmlNode threadHtmlNode, out HtmlNode actualEpisodesDivElement)
        {
            var spanByDescription = threadHtmlNode.Descendants("span").FirstOrDefault(f => f.InnerText.Contains("Aktuelle Episode(n)"));

            if (spanByDescription == null)
            {
                var seasonPackSpan = threadHtmlNode.Descendants("span").FirstOrDefault(f => f.InnerText.StartsWith("Staffelpack"));
                if (seasonPackSpan != null)
                {
                    var parentDiv = seasonPackSpan.NavigateToElementOfType("div", Model.Enumerations.HtmlNavigationType.Parent);
                    var allEpisodesDiv = parentDiv.NavigateToElementOfType("div", Model.Enumerations.HtmlNavigationType.NextSibling);
                    if (allEpisodesDiv.Descendants().All(f => f.Name != "a"))
                    {
                        allEpisodesDiv = parentDiv;
                    }

                    actualEpisodesDivElement = allEpisodesDiv;
                    return true;
                }
            }
            else
            {
                var divByDescription = spanByDescription.NavigateToElementOfType("div", Model.Enumerations.HtmlNavigationType.Parent);
                actualEpisodesDivElement = divByDescription;
                return true;
            }

            actualEpisodesDivElement = null;
            return false;
        }

        private bool TryReadingSeasonPack(HtmlNode threadHtmlNode, DownloadContext downloadContext, out DownloadEntry seasonPackEntry)
        {
            var seasonPackSpan = threadHtmlNode.Descendants("span").FirstOrDefault(f => f.InnerText.Contains("Staffelpack"));
            if (seasonPackSpan != null)
            {
                var parentDiv = seasonPackSpan.NavigateToElementOfType("div", Model.Enumerations.HtmlNavigationType.Parent);
                var downloadLink = GetExternalLinkUrl(parentDiv);
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