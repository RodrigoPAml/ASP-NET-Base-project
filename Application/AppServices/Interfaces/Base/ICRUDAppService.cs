using Application.Responses;
using Domain.Models.Entities.Base;

namespace Application.AppServices.Interfaces.Base
{
    public interface ICRUDAppService<N, U> where N : Entity where U : Entity
    {
        ResponseBody GetPaged(uint page, uint pageSize, string filters, string orderBy);

        ResponseBody Get(ulong id);

        ResponseBody Create(N newEntity);

        ResponseBody Update(U updatedEntity);

        ResponseBody Delete(ulong id);
    }
}
