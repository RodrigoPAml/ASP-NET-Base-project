using API.Infra.Database;
using API.Models.Entities;
using API.Infra.Base;

namespace API.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(DataBaseContext db) : base(db.Users)
        {
        }
    }
}
