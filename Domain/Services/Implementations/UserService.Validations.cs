using Domain.Enums.System;
using Domain.Exceptions;
using Domain.Models.Entities;

namespace Application.Services.Implementations
{
    public partial class UserService
    {
        public void ValidateLoginExists(User user, ActionTypeEnum actionType, object extraInfo)
        {
            if(_repo.Any(x => x.Id != user.Id && x.Login == user.Login))
                throw new BusinessException("User with this login already exists");
        }
    }
}
