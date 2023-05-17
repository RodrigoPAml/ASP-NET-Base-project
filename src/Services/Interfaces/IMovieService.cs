using API.Infra.Query;
using API.Infra.Responses;
using API.Models.NewEntity;
using API.Models.UpdatedEntity;

namespace API.Services.Interfaces
{
    public interface IMovieService
    {
        public PagedData GetPaged(uint page, uint pageSize, List<UserFilter> userFilters, UserOrderBy orderBy);

        public object Get(ulong id);

        public ulong Create(NewMovie newEntity);

        public void Update(UpdatedMovie updatedEntity);

        public void Delete(ulong id);
    }
}
