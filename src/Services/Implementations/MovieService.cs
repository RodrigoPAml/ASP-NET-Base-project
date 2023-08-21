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
    public partial class MovieService : BaseService<Movie>, IMovieService
    {
        public MovieService(IServiceProvider provider, MovieRepository repo) : base(repo, provider)
        {
            AddAction(ValidateMovieExists, ActionTypeEnum.Create);
            AddAction(ValidateMovieExists, ActionTypeEnum.Update);
            AddAction(DeleteDependencies, ActionTypeEnum.Delete);
        }

        public PagedData GetPaged(uint page, uint pageSize, List<UserFilter> userFilters, UserOrderBy orderBy)
        {
            Filter<Movie> filter = new Filter<Movie>();

            // Fields allowed to get filtered
            Fields<Movie> allowedFields = new Fields<Movie>();
            allowedFields.AddAllFields();

            UserFilter.Validate(userFilters, allowedFields);
            UserFilter.Compose(userFilters, filter);

            UserOrderBy.Validate(orderBy, allowedFields);
            var order = UserOrderBy.Compose<Movie>(orderBy);

            return GetPaged(
                page,
                pageSize,
                filter,
                x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    Synopsis = x.Synopsis,
                    Duration = x.Duration,
                    Genre = x.Genre
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
                    Synopsis = x.Synopsis,
                    Duration = x.Duration,
                    Genre = x.Genre
                })
                .FirstOrDefault();

            if(user == null)
                throw new BusinessException("Movie not found");

            return user;
        }

        public ulong Create(NewMovie newEntity)
        {
            newEntity.ValidateWithError();

            var entity = ClassMapper.Map<Movie>(newEntity);

            if (entity == null)
                throw new InternalException($"Mapping failure between {nameof(Movie)} and {nameof(NewMovie)}");
            
            Create(entity);

            return entity.Id;
        }

        public void Update(UpdatedMovie updatedEntity)
        {
            if (!_repo.Any(x => x.Id == updatedEntity.Id))
                throw new BusinessException("Movie not found");

            updatedEntity.ValidateWithError();

            // Fields to update
            Fields<Movie> fields = new Fields<Movie>();
            fields.AddAllFieldsExcept<UpdatedMovie>(x => x.Id);

            var entity = ClassMapper.Map<Movie>(updatedEntity);

            if (entity == null)
                throw new InternalException($"Mapping failure between {nameof(Movie)} and {nameof(UpdatedMovie)}");

            Update(entity, fields);
        }

        public void Delete(ulong id)
        {
            if (!_repo.Any(x => x.Id == id))
                throw new BusinessException("Movie not found");

            base.Delete(id);
        }
    }
}
