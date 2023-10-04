using Domain.Models.Entities.Base;
using Domain.Query;
using System.Linq.Expressions;

namespace Domain.Repositories.Base
{
    public interface IRepository <T> where T : Entity, new()
    {
        IQueryable<T> Where(Filter<T> filter);

        IQueryable<T> Where(Expression<Func<T, bool>> filter);

        bool Any(Filter<T> filter);

        bool Any(Expression<Func<T, bool>> filter);

        void Create(T entity);

        void Update(T entity, Fields<T> fieldsToUpdate);

        void BulkDelete(Filter<T> filter);

        void BulkDelete(Expression<Func<T, bool>> filter);

        public void Delete(T entity);

        public void Delete(ulong id);
    }
}
