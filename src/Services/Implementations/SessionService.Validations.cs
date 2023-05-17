using API.Models.Entities;
using API.Infra.Exceptions;
using API.Infra.Enums;
using API.Repositories;

namespace API.Services.Implementations
{
    public partial class SessionService
    {
        public void ValidateMovieExists(Session session, ActionTypeEnum actionType, object actionInfo)
        {
            var movieRepository = _provider.GetService<MovieRepository>();

            if (!movieRepository.Any(x => x.Id == session.MovieId))
                throw new BusinessException("The provided movie do not exist");
        }
    }
}
