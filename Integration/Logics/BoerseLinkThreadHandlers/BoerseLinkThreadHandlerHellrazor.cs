using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using MMU.BoerseDownloader.Integration.Infrastructure.Extensions;
using MMU.BoerseDownloader.Integration.Logics.Factories;
using MMU.BoerseDownloader.Model;
using MMU.BoerseDownloader.Model.Enumerations;

namespace MMU.BoerseDownloader.Integration.Logics.BoerseLinkThreadHandlers
{
    public class BoerseLinkThreadHandlerHellrazor : BoerseLinkThreadHandler
    {
        public BoerseLinkThreadHandlerHellrazor(WebBrowserAdapterFactory webBrowserAdapterFactory) : base(webBrowserAdapterFactory)
        {
        }

        internal override BoerseLinkProvider BoerseLinkProvider
        {
            get
            {
                return BoerseLinkProvider.Hellraz0r;
            }
        }

        private static HtmlNode GetExternalLinkElement(HtmlNode spanElementForUploaded)
        {
            var nextDiv = spanElementForUploaded.NextSibling;
            while (nextDiv.Name != "div")
            {
                nextDiv = nextDiv.NextSibling;
            }

            var firstLink = nextDiv.Descendants("a").First();
            return firstLink;
        }

        private static string GetExternalUrl(HtmlNode linkElement)
        {
            var result = linkElement.Attributes.First(f => f.Name == "href").Value;
            return result;
        }

        private static HtmlNode TryGetSpanElementForUploadedNet(HtmlNode spoilerContainer)
        {
            const string UPLOADED_NAME = "Uploaded.net";
            var result = spoilerContainer.Descendants("span").SelectMany(g => g.Descendants()).FirstOrDefault(f => f.InnerHtml.Contains(UPLOADED_NAME));
            if (result != null)
            {
                return result;
            }

            result = spoilerContainer.NavigateToElementOfType("#text", Model.Enumerations.HtmlNavigationType.PreviousSibling);
            bool uploadedFound = false;

            while (!uploadedFound)
            {
                if (result.InnerHtml.Contains(UPLOADED_NAME))
                {
                    uploadedFound = true;
                }
                else if (result.InnerHtml != "\n")
                {
                    break;
                }
                else
                {
                    result = result.NavigateToElementOfType("#text", Model.Enumerations.HtmlNavigationType.PreviousSibling);
                }
            }

            if (!uploadedFound)
            {
                result = null;
            }

            return result;
        }

        protected override IReadOnlyCollection<DownloadEntry> ReadAllDownloadEntriesFromThread(HtmlNode threadHtmlNode, DownloadContext downloadContext)
        {
            var res = new List<DownloadEntry>();

            var spoilerContainers = threadHtmlNode.Descendants().Where(f => f.Attributes["Class"]?.Value.Contains("bbCodeSpoilerContainer") == true);

            foreach (var spoilerContainer in spoilerContainers)
            {
                var spanElementForUploaded = TryGetSpanElementForUploadedNet(spoilerContainer);
                if (spanElementForUploaded != null)
                {
                    var linkElement = GetExternalLinkElement(spanElementForUploaded);

                    var entryTitle = linkElement.InnerText;
                    var externalLinkUrl = GetExternalUrl(linkElement);

                    var uploadedDate = GetLastThreadUpdate(threadHtmlNode);

                    var newEntry = new DownloadEntry(downloadContext.Name, externalLinkUrl, entryTitle, uploadedDate, false);
                    res.Add(newEntry);
                }
            }

            return res.AsReadOnly();
        }
    }
}