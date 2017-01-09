using Microsoft.EntityFrameworkCore;

namespace MMU.BoerseDownloader.SqlDataAccess.Mappings
{
    internal interface IEntityMap
    {
        void CreateMap(ModelBuilder modelBuilder);
    }
}