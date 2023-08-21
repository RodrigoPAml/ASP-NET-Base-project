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
            allowedFields.AddAllFields();

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

        public object Get(ulong id)
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

        public ulong Create(NewUser newEntity)
        {
            newEntity.ValidateWithError();

            var entity = ClassMapper.Map<User>(newEntity);

            if (entity == null)
                throw new InternalException($"Mapping failure between {nameof(User)} and {nameof(NewUser)}");
            
            entity.Password = Infra.Utils.BCrypt.EncryptPassword(newEntity.Password);
            Create(entity);

            return entity.Id;
        }

        public void Update(UpdatedUser updatedEntity)
        {
            if (!_repo.Any(x => x.Id == updatedEntity.Id))
                throw new BusinessException("User not found");

            updatedEntity.ValidateWithError();

            Fields<User> fields = new Fields<User>();
            fields.AddAllFieldsExcept<UpdatedUser>(x => x.Id);

            if (string.IsNullOrEmpty(updatedEntity.Password))
                fields.RemoveField(x => x.Password);

            var entity = ClassMapper.Map<User>(updatedEntity);

            if (entity == null)
                throw new InternalException($"Mapping failure between {nameof(User)} and {nameof(UpdatedUser)}");

            Update(entity, fields);
        }

        public void Delete(ulong id)
        {
            if (!_repo.Any(x => x.Id == id))
                throw new BusinessException("User not found");

            base.Delete(id);
        }
    }
}
