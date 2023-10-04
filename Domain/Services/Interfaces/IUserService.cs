using Domain.Models.Entities;
using Domain.Query;
using Domain.Services.Base;

namespace Domain.Services.Interfaces
{
    public interface IUserService : ICRUDService<User>
    {
        List<dynamic> Get(Filter<User> filter, Select<User> select);
    }
}
