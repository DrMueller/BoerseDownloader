using System.Collections.Generic;
using MMU.BoerseDownloader.Model;
using MMU.BoerseDownloader.SqlDataAccess;

namespace MMU.BoerseDownloader.Logics
{
    public class DownloadEntryConfigurationLogic
    {
        private readonly Repository<DownloadEntryConfiguration> _repository;

        public DownloadEntryConfigurationLogic(Repository<DownloadEntryConfiguration> repository)
        {
            _repository = repository;
        }

        public IReadOnlyCollection<DownloadEntryConfiguration> GetAllConfigurationEntries()
        {
            return _repository.LoadAll();
        }

        public void SaveDownloadEntryConfiguration(DownloadEntryConfiguration config)
        {
            _repository.Save(config);
        }
    }
}