using MMU.BoerseDownloader.WpfUI.Models.Enumerations;

namespace MMU.BoerseDownloader.WpfUI.Handlers.InformationHandling
{
    public interface IInformationHandler
    {
        void HandleInformation(InformationType informationType, string message, int? displaySeconds = null);
    }
}