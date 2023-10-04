using Application.AppServices.Interfaces.Base;
using Application.Models.NewEntity;
using Application.Models.UpdatedEntity;

namespace Application.AppServices.Interfaces
{
    public interface ISessionAppService : ICRUDAppService<NewSession, UpdatedSession>
    {
    }
}
