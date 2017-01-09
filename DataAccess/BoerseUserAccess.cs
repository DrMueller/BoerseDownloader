using MMU.BoerseDownloader.DataAccess.Handler;
using MMU.BoerseDownloader.Model;

namespace MMU.BoerseDownloader.DataAccess
{
    public class BoerseUserAccess
    {
        private const string FILE_NAME = "BoerseUser.txt";
        private readonly FileHandler<BoerseUser> _fileHandler;

        public BoerseUserAccess(FileHandler<BoerseUser> fileHandler)
        {
            _fileHandler = fileHandler;
        }

        public bool UserIsConfigured
        {
            get
            {
                return _fileHandler.FileExists(FILE_NAME);
            }
        }

        public BoerseUser Load()
        {
            return _fileHandler.LoadFromFile(FILE_NAME);
        }

        public void Save(BoerseUser boerseUser)
        {
            _fileHandler.SaveFile(boerseUser, FILE_NAME);
        }
    }
}