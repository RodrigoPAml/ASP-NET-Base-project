using Domain.Services.Base;
using Domain.Services.Interfaces;
using Domain.Enums.System;
using Domain.Exceptions;
using Domain.Models.Entities;
using Domain.Query;
using Domain.Repositories.Interfaces;
using Domain.Responses;

namespace Application.Services.Implementations
{
    public partial class SessionService : BaseService<Session>, ISessionService
    {
        public SessionService(IServiceProvider provider, ISessionRepository repo) : base(repo, provider)
        {
            AddAction(ValidateMovieExists, ActionTypeEnum.Create);
            AddAction(ValidateMovieExists, ActionTypeEnum.Update);
        }

        public PagedData GetPaged(uint page, uint pageSize, Filter<Session> filter, OrderBy<Session> order, Select<Session> select)
        {
            return GetPaged(
                page,
                pageSize,
                filter,
                select,
                order
            );
        }

        public object Get(ulong id, Select<Session> select)
        {
            var user =
                Get(
                    x => x.Id == id,
                    select
                )
                .FirstOrDefault();

            if (user == null)
                throw new BusinessException("Session not found");

            return user;
        }

        public ulong Create(Session newEntity)
        {
            base.Create(newEntity);
            return newEntity.Id;
        }

        public void Update(Session updatedEntity, Fields<Session> fields)
        {
            base.Update(updatedEntity, fields);
        }

        public void Delete(ulong id)
        {
            base.Delete(id);
        }
    }
}
