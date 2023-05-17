using API.Models.Entities;
using API.Infra.Exceptions;
using API.Infra.Enums;

namespace API.Services.Implementations
{
    public partial class UserService
    {
        public void ValidateLoginExists(User user, ActionTypeEnum actionType, object actionInfo)
        {
            if(_repo.Any(x => x.Id != user.Id && x.Login == user.Login))
                throw new BusinessException("User with this login already exists");
        }
    }
}
