using System.Linq;
using MMU.BoerseDownloader.Model;
using MMU.BoerseDownloader.SqlDataAccess;

namespace MMU.BoerseDownloader.Logics
{
    public class BoerseUserConfigurationLogic
    {
        private readonly Repository<BoerseUser> _repository;

        public BoerseUserConfigurationLogic(Repository<BoerseUser> repository)
        {
            _repository = repository;
        }

        public bool UserIsConfigured => _repository.LoadAll().Any();

        public BoerseUser Load()
        {
            var result = _repository.LoadAll().SingleOrDefault() ?? new BoerseUser();
            return result;
        }

        public void Save(BoerseUser boerseUser)
        {
            _repository.Save(boerseUser);
        }
    }
}