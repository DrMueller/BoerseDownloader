using System;
using System.Collections.Generic;

namespace MMU.BoerseDownloader.WpfUI.Handlers.ExceptionHandling.Implementation
{
    public class ExceptionHandlerConfiguration : IExceptionHandlerConfiguration
    {
        private readonly List<Action<Exception>> _exceptionCallbacks = new List<Action<Exception>>();

        public void AddExceptionCallback(Action<Exception> exceptionCallback)
        {
            _exceptionCallbacks.Add(exceptionCallback);
        }

        public IReadOnlyCollection<Action<Exception>> ExceptionCallbacks => _exceptionCallbacks;
    }
}