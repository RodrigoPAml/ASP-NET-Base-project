using API.Infra.Database;
using API.Models.Entities;
using API.Infra.Base;

namespace API.Repositories
{
    public class MovieRepository : Repository<Movie>
    {
        public MovieRepository(DataBaseContext db) : base(db.Movies)
        {
        }
    }
}
