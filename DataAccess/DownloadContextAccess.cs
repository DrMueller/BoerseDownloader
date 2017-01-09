using System.Collections.Generic;
using MMU.BoerseDownloader.DataAccess.Handler;
using MMU.BoerseDownloader.Model;

namespace MMU.BoerseDownloader.DataAccess
{
    public class DownloadContextAccess
    {
        private readonly FileHandler<DownloadContext> _fileHandler;

        public DownloadContextAccess(FileHandler<DownloadContext> fileNameHandler)
        {
            _fileHandler = fileNameHandler;
        }

        public IReadOnlyCollection<DownloadContext> LoadAll()
        {
            return _fileHandler.LoadAllFiles();
        }
    }
}