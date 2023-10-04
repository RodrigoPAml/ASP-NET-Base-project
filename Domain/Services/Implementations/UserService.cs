using Domain.Services.Base;
using Domain.Services.Interfaces;
using Domain.Enums.System;
using Domain.Exceptions;
using Domain.Models.Entities;
using Domain.Models.Validators;
using Domain.Query;
using Domain.Repositories.Interfaces;
using Domain.Responses;
using Domain.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.Implementations
{
    public partial class UserService : BaseService<User>, IUserService
    {
        public UserValidator validator = new UserValidator();

        public UserService(IServiceProvider provider, IUserRepository repo) : base(repo, provider)
        {
            AddAction(ValidateLoginExists, ActionTypeEnum.Create);
        }

        public PagedData GetPaged(uint page, uint pageSize, Filter<User> filter, OrderBy<User> order, Select<User> select)
        {
            return GetPaged(
                page,
                pageSize,
                filter,
                select,
                order
            );
        }

        public object Get(ulong id, Select<User> select)
        {
            var user =
                Get(
                    x => x.Id == id, 
                    select
                )
                .FirstOrDefault();

            if(user == null)
                throw new BusinessException("User not found");

            return user;
        }

        public ulong Create(User newEntity)
        {
            validator.ValidateWithError(newEntity, ActionTypeEnum.Create);

            var hasher = _provider.GetService<IPasswordHasherProvider>();
            
            newEntity.Password = hasher.Hash(newEntity.Password);
            base.Create(newEntity);

            return newEntity.Id;
        }

        public void Update(User updatedEntity, Fields<User> fields)
        {
            validator.ValidateWithError(updatedEntity, ActionTypeEnum.Update, fields);

            if(fields.ContainsField(x => x.Password))
            {
                var hasher = _provider.GetService<IPasswordHasherProvider>();
                updatedEntity.Password = hasher.Hash(updatedEntity.Password);
            }

            base.Update(updatedEntity, fields);
        }

        public void Delete(ulong id)
        {
            base.Delete(id);
        }
    }
}
