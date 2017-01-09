using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MMU.BoerseDownloader.SqlDataAccess.Mappings;

namespace MMU.BoerseDownloader.SqlDataAccess.Helpers
{
    internal class ModelMappingRegistrationHelper
    {
        internal static void LoadMapsByReflection(ModelBuilder modelBuilder)
        {
            var allMaps = CreateAllMapInstances();
            foreach (var map in allMaps)
            {
                map.CreateMap(modelBuilder);
            }
        }

        private static IEnumerable<IEntityMap> CreateAllMapInstances()
        {
            var result = new List<IEntityMap>();
            var allMapTypes = GetAllEntityMapTypes();

            foreach (var mapType in allMapTypes)
            {
                var mapInstance = (IEntityMap)Activator.CreateInstance(mapType);
                result.Add(mapInstance);
            }

            return result;
        }

        private static IEnumerable<Type> GetAllEntityMapTypes()
        {
            var entityMapType = typeof(IEntityMap);
            var result = entityMapType.Assembly.GetTypes().Where(f => entityMapType.IsAssignableFrom(f) && !f.IsInterface);

            return result;
        }
    }
}