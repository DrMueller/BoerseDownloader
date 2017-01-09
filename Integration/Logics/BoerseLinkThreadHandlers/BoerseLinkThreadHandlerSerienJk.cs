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
    public class BoerseLinkThreadHandlerSerienJk : BoerseLinkThreadHandler
    {
        public BoerseLinkThreadHandlerSerienJk(WebBrowserAdapterFactory webBrowserAdapterFactory) : base(webBrowserAdapterFactory)
        {
        }

        internal override BoerseLinkProvider BoerseLinkProvider
        {
            get
            {
                return BoerseLinkProvider.SerienJk;
            }
        }

        private bool CheckIfIsAllEpisodesSpan(HtmlNode uploadedSpan)
        {
            const string ALL_PREVIOUS_EPISODES = "Alle bisherigen Episoden";

            var firstDiv = uploadedSpan.GetNextElementOfType("div", Model.Enumerations.HtmlNavigationType.Parent);
            if (firstDiv.InnerText.Contains(ALL_PREVIOUS_EPISODES))
            {
                return true;
            }

            return false;
        }

        private DownloadEntry CreateEntry(HtmlNode threadHtmlNode, DownloadContext downloadContext, HtmlNode uploadedSpan)
        {
            var linkElement = uploadedSpan.ParentNode;

            var externalLink = GetExternalUrl(linkElement);

            string entryName;
            bool isSeasonPack;

            if (CheckIfIsAllEpisodesSpan(uploadedSpan))
            {
                entryName = Constants.DOWNLOADENTRYTITLE_SEASONPACK;
                isSeasonPack = true;
            }
            else
            {
                entryName = GetDownloadEntryTitle(linkElement);
                isSeasonPack = false;
            }

            var lastUploadDate = GetLastThreadUpdate(threadHtmlNode);
            var result = new DownloadEntry(downloadContext.Name, externalLink, entryName, lastUploadDate, isSeasonPack);

            return result;
        }

        private static string GetDownloadEntryTitle(HtmlNode linkElement)
        {
            var parentBlockElement = linkElement.ParentNode;
            var previousBlockElement = parentBlockElement.GetNextElementOfType("b", Model.Enumerations.HtmlNavigationType.PreviousSibling);
            var result = previousBlockElement.InnerText;
            return result;
        }

        private static string GetExternalUrl(HtmlNode linkElement)
        {
            var result = linkElement.Attributes.First(f => f.Name == "href").Value;
            return result;
        }

        protected override IReadOnlyCollection<DownloadEntry> ReadAllDownloadEntriesFromThread(HtmlNode threadHtmlNode, DownloadContext downloadContext)
        {
            var res = new List<DownloadEntry>();

            var uploadedSpans = threadHtmlNode.Descendants("span").Where(f => f.InnerText == "Uploaded");

            foreach (var uploadedSpan in uploadedSpans)
            {
                var newEntry = CreateEntry(threadHtmlNode, downloadContext, uploadedSpan);
                res.Add(newEntry);
            }

            return res.AsReadOnly();
        }
    }
}