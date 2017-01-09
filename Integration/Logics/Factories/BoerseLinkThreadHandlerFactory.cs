using System.Linq;
using MMU.BoerseDownloader.Integration.Logics.BoerseLinkThreadHandlers;
using MMU.BoerseDownloader.Model.Enumerations;

namespace MMU.BoerseDownloader.Integration.Logics.Factories
{
    public class BoerseLinkThreadHandlerFactory
    {
        private readonly BoerseLinkThreadHandler[] _threadHandlers;

        public BoerseLinkThreadHandlerFactory(BoerseLinkThreadHandler[] threadHandlers)
        {
            _threadHandlers = threadHandlers;
        }

        internal BoerseLinkThreadHandler Create(BoerseLinkProvider boerseLinkProvider)
        {
            var threadHandler = _threadHandlers.First(f => f.BoerseLinkProvider == boerseLinkProvider);
            return threadHandler;
        }
    }
}