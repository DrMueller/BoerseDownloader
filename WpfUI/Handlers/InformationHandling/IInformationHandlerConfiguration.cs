using System;
using System.Collections.Generic;
using MMU.BoerseDownloader.WpfUI.Models.Enumerations;

namespace MMU.BoerseDownloader.WpfUI.Handlers.InformationHandling
{
    public interface IInformationHandlerConfiguration
    {
        IReadOnlyCollection<Action<InformationType, string, int?>> InformationCallbacks { get; }

        void AddInformationCallback(Action<InformationType, string, int?> informationCallback);
    }
}