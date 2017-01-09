using System.Diagnostics;
using MMU.BoerseDownloader.WpfUI.Models.Enumerations;

namespace MMU.BoerseDownloader.WpfUI.Handlers.InformationHandling.Implementation
{
    public class InformationHandler : IInformationHandler
    {
        private readonly IInformationHandlerConfiguration _informationHandlerConfiguration;

        public InformationHandler(IInformationHandlerConfiguration informationHandlerConfiguration)
        {
            _informationHandlerConfiguration = informationHandlerConfiguration;
        }

        public void HandleInformation(InformationType informationType, string message, int? displaySeconds)
        {
            foreach (var cb in _informationHandlerConfiguration.InformationCallbacks)
            {
                cb.Invoke(informationType, message, displaySeconds);
            }
        }
    }
}