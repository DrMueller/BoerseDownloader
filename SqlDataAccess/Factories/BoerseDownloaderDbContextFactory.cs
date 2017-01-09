using Microsoft.EntityFrameworkCore;
using Microsoft.Practices.Unity;

namespace MMU.BoerseDownloader.SqlDataAccess.Factories
{
    public class BoerseDownloaderDbContextFactory
    {
        private readonly IUnityContainer _unityContainer;

        public BoerseDownloaderDbContextFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        internal BoerseDownloaderDbContext Create()
        {
            var result = _unityContainer.Resolve<BoerseDownloaderDbContext>();
            result.ChangeTracker.AutoDetectChangesEnabled = false;

            // http://stackoverflow.com/questions/31216983/update-database-after-model-changes-entity-framework-7
            result.Database.Migrate();

            return result;
        }
    }
}