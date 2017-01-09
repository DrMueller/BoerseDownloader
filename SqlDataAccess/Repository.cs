using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MMU.BoerseDownloader.Model.Interfaces;
using MMU.BoerseDownloader.SqlDataAccess.Factories;

namespace MMU.BoerseDownloader.SqlDataAccess
{
    public class Repository<TEntity>
        where TEntity : class, IIdentifiable
    {
        private readonly BoerseDownloaderDbContextFactory _contextFactory;

        public Repository(BoerseDownloaderDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            ExecuteActionInDbContext(dbSet =>
            {
                var entries = dbSet.Where(predicate);
                dbSet.RemoveRange(entries);
            });
        }

        public void DeleteAll()
        {
            ExecuteActionInDbContext(dbSet =>
            {
                dbSet.RemoveRange(dbSet);
            });
        }

        public void DeletebyId(long id)
        {
            ExecuteActionInDbContext(dbSet =>
            {
                var entry = dbSet.First(f => f.Id == id);
                dbSet.Remove(entry);
            });
        }

        public IReadOnlyCollection<TEntity> Load(Expression<Func<TEntity, bool>> predicate)
        {
            var entries = ReadFromDbContext(f => f.Where(predicate)).ToList();
            return entries.AsReadOnly();
        }

        public IReadOnlyCollection<TEntity> LoadAll()
        {
            var allEntries = ReadFromDbContext(f => f.ToList());
            return allEntries.AsReadOnly();
        }

        public void Save(TEntity entity)
        {
            ExecuteActionInDbContext(dbSet =>
            {
                if (entity.Id == 0)
                {
                    dbSet.Add(entity);
                }
                else
                {
                    dbSet.Update(entity);
                }
            });
        }

        private void ExecuteActionInDbContext(Action<DbSet<TEntity>> action)
        {
            using (var context = _contextFactory.Create())
            {
                var dbSet = context.Set<TEntity>();
                action(dbSet);
                context.SaveChanges();
            }
        }

        private T ReadFromDbContext<T>(Func<DbSet<TEntity>, T> func)
        {
            using (var context = _contextFactory.Create())
            {
                var dbSet = context.Set<TEntity>();
                return func(dbSet);
            }
        }
    }
}