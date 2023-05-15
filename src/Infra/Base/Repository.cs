using API.Infra.Query;
using Microsoft.EntityFrameworkCore;
using API.Infra.Extensions;
using System.Linq.Expressions;

namespace API.Infra.Base
{
    /// <summary>
    /// Base repository class with common operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> where T : Entity, new()
    {
        private readonly DbSet<T> _dbSet;

        public Repository(DbSet<T> dbSet)
        {
            _dbSet = dbSet;
        }

        public IQueryable<T> Where(Filter<T> filter)
        {
            if (filter == null)
                return _dbSet.Where(x => true);

            return _dbSet.Where(filter.GetExpression());
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
                return _dbSet.Where(x => true);

            return _dbSet.Where(filter);
        }

        public bool Any(Filter<T> filter)
        {
            return _dbSet.Any(filter.GetExpression());
        }

        public bool Any(Expression<Func<T, bool>> filter)
        {
            return _dbSet.Any(filter);
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity, Fields<T> fieldsToUpdate)
        {
            var count = fieldsToUpdate.Count();

            if (count == 0)
                return;

            var entry = _dbSet.Entry(entity);

            _dbSet.Attach(entity);
        
            foreach (var property in fieldsToUpdate.GetNames(false))
                entry.Property(property).IsModified = true;
        }

        public void BulkDelete(Filter<T> filter)
        {
            _dbSet.Where(filter.GetExpression()).ExecuteDelete();
        }

        public void BulkDelete(Expression<Func<T, bool>> filter)
        {
            _dbSet.Where(filter).ExecuteDelete();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Delete(ulong id)
        {
            _dbSet.Remove(new T()
            {
                Id = id
            });
        }
    }
}
