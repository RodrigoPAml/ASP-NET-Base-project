using API.Infra.Query;
using API.Infra.Responses;
using API.Models.NewEntity;
using API.Models.UpdatedEntity;

namespace API.Services.Interfaces
{
    public interface IUserService
    {
        public PagedData GetPaged(uint page, uint pageSize, List<UserFilter> userFilters, UserOrderBy orderBy);

        public object GetUser(ulong id);

        public void CreateUser(NewUser newUser);

        public void UpdateUser(UpdatedUser updatedUser);

        public void DeleteUser(ulong id);
    }
}
