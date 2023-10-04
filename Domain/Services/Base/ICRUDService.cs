using Domain.Models.Entities.Base;
using Domain.Query;
using Domain.Responses;

namespace Domain.Services.Base
{
    /// <summary>
    /// Interface for CRUD operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICRUDService<T> where T : Entity, new()
    {
        PagedData GetPaged(uint page, uint pageSize, Filter<T> filters, OrderBy<T> orderBy, Select<T> select);

        object Get(ulong id, Select<T> select);

        ulong Create(T newEntity);

        void Update(T updatedEntity, Fields<T> fields);

        void Delete(ulong id);
    }
}
