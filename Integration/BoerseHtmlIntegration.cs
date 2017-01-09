using System.Collections.Generic;
using MMU.BoerseDownloader.Integration.Logics.Factories;
using MMU.BoerseDownloader.Model;

namespace MMU.BoerseDownloader.Integration
{
    public class BoerseHtmlIntegration
    {
        private readonly BoerseLinkThreadHandlerFactory _boerseLinkThreadHandlerFactory;

        public BoerseHtmlIntegration(BoerseLinkThreadHandlerFactory boerseLinkThreadHandlerFactory)
        {
            _boerseLinkThreadHandlerFactory = boerseLinkThreadHandlerFactory;
        }

        public IReadOnlyCollection<DownloadEntry> Download(DownloadContext downloadContext, BoerseUser boerseUser)
        {
            var threadHandler = _boerseLinkThreadHandlerFactory.Create(downloadContext.BoerseLinkProvider);
            var result = threadHandler.GetAllEntries(downloadContext, boerseUser);
            return result;
        }
    }
}