using API.Infra.Database;
using API.Models.Entities;
using API.Infra.Base;

namespace API.Repositories
{
    public class SessionRepository : Repository<Session>
    {
        public SessionRepository(DataBaseContext db) : base(db.Sessions)
        {
        }
    }
}
