using Microsoft.EntityFrameworkCore;
using MMU.BoerseDownloader.Model;

namespace MMU.BoerseDownloader.SqlDataAccess.Mappings.Implementation
{
    public class DownloadEntryConfigurationMap : IEntityMap
    {
        private const string TABLE_NAME = "DownloadEntryConfiguration";

        public void CreateMap(ModelBuilder modelBuilder)
        {
            var entityBuilder = modelBuilder.Entity<DownloadEntryConfiguration>();

            entityBuilder.HasKey(f => f.Id);
            entityBuilder.Property(f => f.IsLinkVisited).IsRequired();
            entityBuilder.Property(f => f.Title).IsRequired();
            entityBuilder.Property(f => f.DownloadLinkIdentifier);

            entityBuilder.ToTable(TABLE_NAME, Infrastructure.Extensions.CORE_SCHEMA);
        }
    }
}