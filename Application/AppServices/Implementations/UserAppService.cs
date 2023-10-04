using Application.AppServices.Interfaces;
using Application.Mapper;
using Application.Models.NewEntity;
using Application.Models.UpdatedEntity;
using Application.Query;
using Application.Responses;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.Models.Entities;
using Domain.Persistance;
using Domain.Query;
using Domain.Security;
using Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Application.AppServices.Implementations
{
    public class UserAppService : IUserAppService
    {
        private IUserService _service;

        private IDatabaseTransaction _transaction;

        private IServiceProvider _provider;

        public UserAppService(IUserService service, IDatabaseTransaction transaction, IServiceProvider provider)
        {
            _service = service;
            _transaction = transaction; 
            _provider = provider;
        }

        public ResponseBody GetPaged(uint page, uint pageSize, string filters, string orderBy)
        {
            try
            {
                // Fields allowed to get filtered from front-end
                Fields<User> allowedFields = new Fields<User>();
                allowedFields.AddAllFields();

                var filter = UserFilter.Compose(filters, allowedFields);
                var order = UserOrderBy.Compose(orderBy, allowedFields);

                var list = _service.GetPaged(
                    page,
                    pageSize,
                    filter,
                    order,
                    new Select<User>(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Login,
                        x.Profile,
                    })
                );

                return ResponseBody.WithContentSuccess("Records retrieved successfully", list);
            }
            catch (BusinessException be)
            {
                return ResponseBody.HandledError(be);
            }
            catch (Exception e)
            {
                return ResponseBody.UnhandledError(e);
            }
        }

        public ResponseBody Get(ulong id)
        {
            try
            {
                var user = _service.Get(
                    id,
                    new Select<User>(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Profile,
                        x.Login,
                    })
                );

                if (user == null)
                    throw new BusinessException("User not found");

                return ResponseBody.WithContentSuccess("Updated with success", user);
            }
            catch (BusinessException be)
            {
                return ResponseBody.HandledError(be);
            }
            catch (Exception e)
            {
                return ResponseBody.UnhandledError(e);
            }
        }

        public ResponseBody Create(NewUser newEntity)
        {
            try
            {
                var entity = ClassMapper.Map<User>(newEntity);

                if (entity == null)
                    throw new InternalException($"Mapping failure between {nameof(User)} and {nameof(NewUser)}");

                entity.Profile = ProfileTypeEnum.User;
                _transaction.Begin();
                
                _service.Create(entity);
                
                _transaction.Save();
                _transaction.Commit();

                return ResponseBody.WithContentSuccess("Updated with success", entity.Id);
            }
            catch (BusinessException be)
            {
                return ResponseBody.HandledError(be);
            }
            catch (Exception e)
            {
                return ResponseBody.UnhandledError(e);
            }
        }

        public ResponseBody Update(UpdatedUser updatedEntity)
        {
            try
            {
                Fields<User> fields = new Fields<User>();
                fields.AddAllFieldsExcept<UpdatedUser>(x => x.Id);

                if (string.IsNullOrEmpty(updatedEntity.Password))
                    fields.RemoveField(x => x.Password);

                var entity = ClassMapper.Map<User>(updatedEntity);

                if (entity == null)
                    throw new InternalException($"Mapping failure between {nameof(User)} and {nameof(UpdatedUser)}");

                _transaction.Begin();
                _service.Update(entity, fields);
                _transaction.Save();
                _transaction.Commit();

                return ResponseBody.NoContentSuccess("Updated with success");
            }
            catch (BusinessException be)
            {
                return ResponseBody.HandledError(be);
            }
            catch (Exception e)
            {
                return ResponseBody.UnhandledError(e);
            }
        }

        public ResponseBody Delete(ulong id)
        {
            try
            {
                _transaction.Begin();

                _service.Delete(id);

                _transaction.Save();
                _transaction.Commit();

                return ResponseBody.NoContentSuccess("Deleted with success");
            }
            catch (BusinessException be)
            {
                return ResponseBody.HandledError(be);
            }
            catch (Exception e)
            {
                return ResponseBody.UnhandledError(e);
            }
        }

        public List<Claim> GetLogin(string login, string password)
        {
            var hasher = _provider.GetService<IPasswordHasherProvider>();

            User? user = _service.Get(
                new Filter<User>(x => x.Login == login), 
                new Select<User>(x => new User
                {
                    Id = x.Id,
                    Name = x.Name,
                    Login = x.Login,
                    Profile = x.Profile,
                    Password = x.Password,
                })
            )
            .FirstOrDefault();

            if (user == null)
                throw new BusinessException("Incorrect login or password");

            if (!hasher.IsValid(password, user.Password))
                throw new BusinessException("Incorrect login or password");

            return new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Login),
                new Claim(ClaimTypes.Role, user.Profile.GetDescription()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };
        }
    }
}
