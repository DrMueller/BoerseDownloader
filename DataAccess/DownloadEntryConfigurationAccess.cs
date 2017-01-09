using System.Collections.Generic;
using MMU.BoerseDownloader.DataAccess.Handler;
using MMU.BoerseDownloader.Model;

namespace MMU.BoerseDownloader.DataAccess
{
    public class DownloadEntryConfigurationAccess
    {
        private readonly FileHandler<DownloadEntryConfigurationLegacy> _fileHandler;

        public DownloadEntryConfigurationAccess(FileHandler<DownloadEntryConfigurationLegacy> fileHandler)
        {
            _fileHandler = fileHandler;
        }

        public IReadOnlyCollection<DownloadEntryConfigurationLegacy> LoadAll()
        {
            return _fileHandler.LoadAllFiles();
        }
    }
}