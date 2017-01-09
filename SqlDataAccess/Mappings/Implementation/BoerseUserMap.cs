using Microsoft.EntityFrameworkCore;
using MMU.BoerseDownloader.Model;

namespace MMU.BoerseDownloader.SqlDataAccess.Mappings.Implementation
{
    internal class BoerseUserMap : IEntityMap
    {
        private const string TABLE_NAME = "BoerseUser";

        public void CreateMap(ModelBuilder modelBuilder)
        {
            var entityBuilder = modelBuilder.Entity<BoerseUser>();

            entityBuilder.HasKey(f => f.Id);
            entityBuilder.Property(f => f.Password).IsRequired();
            entityBuilder.Property(f => f.LoginName).IsRequired();
            entityBuilder.ToTable(TABLE_NAME, Infrastructure.Extensions.CORE_SCHEMA);
        }
    }
}