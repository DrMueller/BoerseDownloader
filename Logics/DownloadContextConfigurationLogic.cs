using System.Collections.Generic;
using MMU.BoerseDownloader.Model;
using MMU.BoerseDownloader.SqlDataAccess;

namespace MMU.BoerseDownloader.Logics
{
    public class DownloadContextConfigurationLogic
    {
        private readonly Repository<DownloadContext> _repository;

        public DownloadContextConfigurationLogic(Repository<DownloadContext> repository)
        {
            _repository = repository;
        }

        public void Delete(long id)
        {
            _repository.DeletebyId(id);
        }

        public IReadOnlyCollection<DownloadContext> LoadAll()
        {
            return _repository.LoadAll();
        }

        public void Save(DownloadContext downloadContext)
        {
            _repository.Save(downloadContext);
        }
    }
}