using Application.AppServices.Interfaces.Base;
using Application.Models.NewEntity;
using Application.Models.UpdatedEntity;
using System.Security.Claims;

namespace Application.AppServices.Interfaces
{
    public interface IUserAppService : ICRUDAppService<NewUser, UpdatedUser>
    {
        List<Claim> GetLogin(string login, string password);
    }
}
