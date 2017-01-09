using System;
using NLog;

namespace MMU.BoerseDownloader.WpfUI.Handlers.ExceptionHandling.Implementation
{
    public class ExceptionLogger : IExceptionLogger
    {
        private const string ERROR_LOGGER = "ERRORS";
        private readonly Logger _logger = LogManager.GetLogger(ERROR_LOGGER);

        public void LogException(Exception ex)
        {
            _logger.Log(LogLevel.Error, ex);
        }
    }
}