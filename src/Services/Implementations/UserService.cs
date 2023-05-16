using API.Models.Entities;
using API.Repositories;
using API.Infra.Base;
using API.Infra.Query;
using API.Services.Interfaces;
using API.Models.NewEntity;
using API.Models.UpdatedEntity;
using API.Infra.Exceptions;
using API.Infra.Enums;
using API.Infra.Mapper;
using API.Infra.Responses;

namespace API.Services.Implementations
{
    public partial class UserService : BaseService<User>, IUserService
    {
        public UserService(IServiceProvider provider, UserRepository repo) : base(repo, provider)
        {
            AddAction(ValidateLoginExists, ActionTypeEnum.Create);
        }

        public PagedData GetPaged(uint page, uint pageSize, List<UserFilter> userFilters, UserOrderBy orderBy)
        {
            Filter<User> filter = new Filter<User>();

            Fields<User> allowedFields = new Fields<User>();
            allowedFields.AddField(x => x.Name);
            allowedFields.AddField(x => x.Login);
            allowedFields.AddField(x => x.Profile);
            allowedFields.AddField(x => x.Password);
            allowedFields.AddField(x => x.Id);

            UserFilter.Validate(userFilters, allowedFields);
            UserFilter.Compose(userFilters, filter);

            UserOrderBy.Validate(orderBy, allowedFields);
            var order = UserOrderBy.Compose<User>(orderBy);

            return GetPaged(
                page,
                pageSize,
                filter,
                x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    Login = x.Login,
                    Profile = x.Profile,
                },
                order
            );
        }

        public object GetUser(ulong id)
        {
            var user = Get(
                x => x.Id == id,
                x => new 
                {
                    Id = x.Id,
                    Name = x.Name,
                    Profile = x.Profile,
                    Login = x.Login,
                })
                .FirstOrDefault();

            if(user == null)
                throw new BusinessException("User not found");

            return user;
        }

        public ulong CreateUser(NewUser newUser)
        {
            newUser.ValidateWithError();

            var entity = ClassMapper.Map<User>(newUser);

            if (entity == null)
                throw new InternalException($"Mapping failure between {nameof(User)} and {nameof(NewUser)}");
            
            entity.Password = Infra.Utils.BCrypt.EncryptPassword(newUser.Password);
            Create(entity);

            return entity.Id;
        }

        public void UpdateUser(UpdatedUser updatedUser)
        {
            if (!_repo.Any(x => x.Id == updatedUser.Id))
                throw new BusinessException("User not found");

            updatedUser.ValidateWithError();

            Fields<User> fields = new Fields<User>();

            if (updatedUser.UpdateName)
                fields.AddField(x => x.Name);

            var entity = ClassMapper.Map<User>(updatedUser);

            if (entity == null)
                throw new InternalException($"Mapping failure between {nameof(User)} and {nameof(UpdatedUser)}");

            Update(entity, fields);
        }

        public void DeleteUser(ulong id)
        {
            if (!_repo.Any(x => x.Id == id))
                throw new BusinessException("User not found");

            Delete(id);
        }
    }
}
