using System;

namespace MMU.BoerseDownloader.WpfUI.Handlers.ExceptionHandling
{
    public interface IExceptionLogger
    {
        void LogException(Exception ex);
    }
}