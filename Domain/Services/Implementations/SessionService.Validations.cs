using Domain.Enums.System;
using Domain.Exceptions;
using Domain.Models.Entities;
using Domain.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.Implementations
{
    public partial class SessionService
    {
        public void ValidateMovieExists(Session session, ActionTypeEnum actionType, object extraInfo)
        {
            var movieRepository = _provider.GetService<IMovieRepository>();

            if (!movieRepository.Any(x => x.Id == session.MovieId))
                throw new BusinessException("The provided movie do not exist");
        }
    }
}
