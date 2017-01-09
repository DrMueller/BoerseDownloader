using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HtmlAgilityPack;
using MMU.BoerseDownloader.Common.Infrastructure;
using MMU.BoerseDownloader.Integration.Logics.Factories;
using MMU.BoerseDownloader.Model;

namespace MMU.BoerseDownloader.Integration.Logics.BoerseLinkThreadHandlers
{
    public abstract class BoerseLinkThreadHandler : IDisposable
    {
        private readonly WebBrowserAdapterFactory _webBrowserAdapterFactory;
        private bool _disposed;

        protected BoerseLinkThreadHandler(WebBrowserAdapterFactory webBrowserAdapterFactory)
        {
            _webBrowserAdapterFactory = webBrowserAdapterFactory;
        }

        internal abstract BoerseDownloader.Model.Enumerations.BoerseLinkProvider BoerseLinkProvider { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal IReadOnlyCollection<DownloadEntry> GetAllEntries(DownloadContext downloadContext, BoerseUser boerseUser)
        {
            var threadNode = DownloadAndProcessHtml(downloadContext, boerseUser);
            var entries = ReadAllDownloadEntriesFromThread(threadNode, downloadContext);
            return entries;
        }

        private HtmlNode DownloadAndProcessHtml(DownloadContext downloadContext, BoerseUser boerseUser)
        {
            const int MAX_WAIT_COUNTER = 10;
            HtmlNode dataAuthorNode;
            var currentCnter = 0;

            var downloadSucceeded = TryDownloadingDataAuthorNode(downloadContext, boerseUser, out dataAuthorNode);

            while (!downloadSucceeded && currentCnter++ < MAX_WAIT_COUNTER)
            {
                Thread.Sleep(2000);
                downloadSucceeded = TryDownloadingDataAuthorNode(downloadContext, boerseUser, out dataAuthorNode);
            }

            if (!downloadSucceeded)
            {
                throw new Exception($"Could not download Thread for {downloadContext.Name}.");
            }

            return dataAuthorNode;
        }

        private string DownloadRawHtml(DownloadContext downloadContext, BoerseUser boerseUser)
        {
            var adapter = _webBrowserAdapterFactory.Create(downloadContext);
            var result = adapter.DownloadThreadHtml(downloadContext, boerseUser);
            return result;
        }

        private bool TryDownloadingDataAuthorNode(DownloadContext downloadContext, BoerseUser boerseUser, out HtmlNode dataAuthorNode)
        {
            const string ATTR_DATA_AUTHOR = "data-author";

            var rawHtml = DownloadRawHtml(downloadContext, boerseUser);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(rawHtml);

            dataAuthorNode = htmlDoc.DocumentNode.Descendants().Where(f => f.Name == "li").FirstOrDefault(f => f.Attributes[ATTR_DATA_AUTHOR]?.Value == downloadContext.BoerseLinkProvider.GetNativeName());
            if (dataAuthorNode == null)
            {
                return false;
            }

            return true;
        }

        private static bool TryParseDateTime(string str, out DateTime dateTime)
        {
            DateTime dt;
            if (DateTime.TryParse(str, out dt))
            {
                dateTime = dt;
                return true;
            }

            dateTime = DateTime.MinValue;
            return false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _webBrowserAdapterFactory.Dispose();
                }

                //// Unmanaged resources are released here.
                _disposed = true;
            }
        }

        protected DateTime? GetLastThreadUpdate(HtmlNode threadHtmlNode)
        {
            var lastEditElement = threadHtmlNode.Descendants().FirstOrDefault(f => f.Attributes["class"]?.Value == "editDate");
            var secondSubElement = lastEditElement?.ChildNodes[1];
            if (secondSubElement != null)
            {
                var uploadedValue = secondSubElement.InnerText;

                // "8. Aug. 2016"
                DateTime parsedDate;

                if (TryParseDateTime(uploadedValue, out parsedDate))
                {
                    return parsedDate;
                }

                var dataTimeString = secondSubElement.Attributes["data-timestring"].Value;
                var dataDateString = secondSubElement.Attributes["data-datestring"].Value;

                uploadedValue = string.Concat(dataDateString, " ", dataTimeString);

                if (TryParseDateTime(uploadedValue, out parsedDate))
                {
                    return parsedDate;
                }

                throw new Exception($"Couldnt parse Date {uploadedValue} for last Upload Date.");
            }

            return null;
        }

        protected abstract IReadOnlyCollection<DownloadEntry> ReadAllDownloadEntriesFromThread(HtmlNode threadHtmlNode, DownloadContext downloadContext);

        ~BoerseLinkThreadHandler()
        {
            Dispose(false);
        }
    }
}