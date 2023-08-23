using API.Models.Entities;
using API.Repositories;
using API.Infra.Base;
using API.Infra.Query;
using API.Services.Interfaces;
using API.Models.NewEntity;
using API.Models.UpdatedEntity;
using API.Infra.Exceptions;
using API.Infra.Mapper;
using API.Infra.Responses;
using API.Infra.Enums;

namespace API.Services.Implementations
{
    public partial class SessionService : BaseService<Session>, ISessionService
    {
        public SessionService(IServiceProvider provider, SessionRepository repo) : base(repo, provider)
        {
            AddAction(ValidateMovieExists, ActionTypeEnum.Create);
            AddAction(ValidateMovieExists, ActionTypeEnum.Update);
        }

        public PagedData GetPaged(uint page, uint pageSize, List<UserFilter> userFilters, UserOrderBy orderBy)
        {
            Filter<Session> filter = new Filter<Session>();

            Fields<Session> allowedFields = new Fields<Session>();
            allowedFields.AddAllFields();

            UserFilter.Validate(userFilters, allowedFields);
            UserFilter.Compose(userFilters, filter);
            
            UserOrderBy.Validate(orderBy, allowedFields);
            var order = UserOrderBy.Compose<Session>(orderBy);

            return GetPaged(
                page,
                pageSize,
                filter,
                x => new
                {
                    Id = x.Id,
                    MovieId = x.MovieId,
                    Movie = new
                    {
                        x.Movie.Name,
                    },
                    Date = x.Date.ToString("dd/MM/yyyy hh:mm"),
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
                    MovieId = x.MovieId,
                    Movie = new
                    {
                        x.Movie.Name,
                    },
                    Date = x.Date
                })
                .FirstOrDefault();

            if(user == null)
                throw new BusinessException("Session not found");

            return user;
        }

        public ulong Create(NewSession newEntity)
        {
            newEntity.ValidateWithError();

            var entity = ClassMapper.Map<Session>(newEntity);

            if (entity == null)
                throw new InternalException($"Mapping failure between {nameof(Session)} and {nameof(NewSession)}");
            
            Create(entity);

            return entity.Id;
        }

        public void Update(UpdatedSession updatedEntity)
        {
            if (!_repo.Any(x => x.Id == updatedEntity.Id))
                throw new BusinessException("Session not found");

            updatedEntity.ValidateWithError();

            Fields<Session> fields = new Fields<Session>();
            fields.AddAllFieldsExcept<UpdatedSession>(x => x.Id);

            var entity = ClassMapper.Map<Session>(updatedEntity);

            if (entity == null)
                throw new InternalException($"Mapping failure between {nameof(Session)} and {nameof(UpdatedSession)}");

            Update(entity, fields);
        }

        public void Delete(ulong id)
        {
            if (!_repo.Any(x => x.Id == id))
                throw new BusinessException("Session not found");

            base.Delete(id);
        }
    }
}
