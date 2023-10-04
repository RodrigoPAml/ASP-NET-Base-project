using Domain.Models.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Domain.Enums.System;
using Domain.Repositories.Interfaces;

namespace Application.Services.Implementations
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
            var sessionRepository = _provider.GetService<ISessionRepository>();

            sessionRepository.BulkDelete(x => x.MovieId == movie.Id);
        }
    }
}
