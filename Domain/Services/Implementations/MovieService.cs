using Domain.Services.Base;
using Domain.Services.Interfaces;
using Domain.Enums.System;
using Domain.Exceptions;
using Domain.Models.Entities;
using Domain.Models.Validators;
using Domain.Query;
using Domain.Repositories.Interfaces;
using Domain.Responses;

namespace Application.Services.Implementations
{
    public partial class MovieService : BaseService<Movie>, IMovieService
    {
        private MovieValidator validator = new MovieValidator();

        public MovieService(IServiceProvider provider, IMovieRepository repo) : base(repo, provider)
        {
            AddAction(ValidateMovieExists, ActionTypeEnum.Create);
            AddAction(ValidateMovieExists, ActionTypeEnum.Update);
            AddAction(DeleteDependencies, ActionTypeEnum.Delete);
        }

        public PagedData GetPaged(uint page, uint pageSize, Filter<Movie> filter, OrderBy<Movie> order, Select<Movie> select)
        {
            return GetPaged(
                page,
                pageSize,
                filter,
                select,
                order
            );
        }

        public object Get(ulong id, Select<Movie> select)
        {
            var user =
                Get(
                    x => x.Id == id,
                    select
                )
                .FirstOrDefault();

            if (user == null)
                throw new BusinessException("Movie not found");

            return user;
        }

        public ulong Create(Movie newEntity)
        {
            validator.ValidateWithError(newEntity, ActionTypeEnum.Create);
            base.Create(newEntity);

            return newEntity.Id;
        }

        public void Update(Movie updatedEntity, Fields<Movie> fields)
        {
            validator.ValidateWithError(updatedEntity, ActionTypeEnum.Update, fields);
            base.Update(updatedEntity, fields);
        }

        public void Delete(ulong id)
        {
            base.Delete(id);
        }
    }
}
