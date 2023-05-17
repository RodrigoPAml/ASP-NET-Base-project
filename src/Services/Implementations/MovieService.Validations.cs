using API.Models.Entities;
using API.Infra.Base;
using API.Services.Interfaces;
using API.Infra.Exceptions;
using API.Infra.Enums;
using API.Repositories;

namespace API.Services.Implementations
{
    public partial class MovieService
    {
        public void ValidateMovieExists(Movie movie, ActionTypeEnum actionType, object actionInfo)
        {
            if(_repo.Any(x => x.Id != movie.Id && x.Name == movie.Name))
                throw new BusinessException("Movie with this name already exists");
        }

        public void DeleteDependencies(Movie movie, ActionTypeEnum actionType, object actionInfo)
        {
            var sessionRepository = _provider.GetService<SessionRepository>();

            sessionRepository.BulkDelete(x => x.MovieId == movie.Id);
            _transaction.Save();
        }
    }
}
