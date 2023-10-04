using Domain.Models.Entities;
using Domain.Repositories.Interfaces;
using Infra.Database;
using Infra.Repositories.Base;

namespace Infra.Repositories.Implementation
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext ctx) : base(ctx.Users)
        {
        }
    }
}
