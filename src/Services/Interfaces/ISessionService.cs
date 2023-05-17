using API.Infra.Query;
using API.Infra.Responses;
using API.Models.NewEntity;
using API.Models.UpdatedEntity;

namespace API.Services.Interfaces
{
    public interface ISessionService
    {
        public PagedData GetPaged(uint page, uint pageSize, List<UserFilter> userFilters, UserOrderBy orderBy);

        public object Get(ulong id);

        public ulong Create(NewSession newEntity);

        public void Update(UpdatedSession updatedEntity);

        public void Delete(ulong id);
    }
}
