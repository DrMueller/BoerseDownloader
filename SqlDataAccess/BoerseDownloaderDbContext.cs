using Microsoft.EntityFrameworkCore;
using MMU.BoerseDownloader.Model;

namespace MMU.BoerseDownloader.SqlDataAccess
{
    internal class BoerseDownloaderDbContext : DbContext
    {
        public DbSet<BoerseUser> BoerseUsers { get; set; }

        public DbSet<DownloadContext> DownloadContexts { get; set; }

        public DbSet<DownloadEntryConfiguration> DownloadEntryConfigurations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = $@"Data Source=(localdb)\mssqllocaldb;Initial Catalog=BoerseDownloader;Integrated Security=True";
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Helpers.ModelMappingRegistrationHelper.LoadMapsByReflection(modelBuilder);
        }
    }
}