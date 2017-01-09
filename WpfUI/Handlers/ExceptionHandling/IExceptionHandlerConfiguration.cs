using System;
using System.Collections.Generic;

namespace MMU.BoerseDownloader.WpfUI.Handlers.ExceptionHandling
{
    public interface IExceptionHandlerConfiguration
    {
        IReadOnlyCollection<Action<Exception>> ExceptionCallbacks { get; }

        void AddExceptionCallback(Action<Exception> exceptionCallback);
    }
}