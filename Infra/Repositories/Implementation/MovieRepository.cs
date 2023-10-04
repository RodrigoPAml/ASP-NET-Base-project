using Domain.Models.Entities;
using Domain.Repositories.Interfaces;
using Infra.Database;
using Infra.Repositories.Base;

namespace Infra.Repositories.Implementation
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(DatabaseContext ctx) : base(ctx.Movies)
        {
        }
    }
}
