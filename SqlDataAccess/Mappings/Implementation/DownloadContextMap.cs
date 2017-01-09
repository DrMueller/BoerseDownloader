using Microsoft.EntityFrameworkCore;
using MMU.BoerseDownloader.Model;

namespace MMU.BoerseDownloader.SqlDataAccess.Mappings.Implementation
{
    internal class DownloadContextMap : IEntityMap
    {
        private const string TABLE_NAME = "DownloadContext";

        public void CreateMap(ModelBuilder modelBuilder)
        {
            var entityBuilder = modelBuilder.Entity<DownloadContext>();

            entityBuilder.HasKey(f => f.Id);
            entityBuilder.Property(f => f.BoerseLinkProvider).IsRequired();
            entityBuilder.Property(f => f.Name).IsRequired();
            entityBuilder.Property(f => f.ThreadUrl).IsRequired();

            entityBuilder.ToTable(TABLE_NAME, Infrastructure.Extensions.CORE_SCHEMA);
        }
    }
}