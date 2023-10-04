using Domain.Models.Entities;
using Domain.Repositories.Interfaces;
using Infra.Database;
using Infra.Repositories.Base;

namespace Infra.Repositories.Implementation
{
    public class SessionRepository : Repository<Session>, ISessionRepository
    {
        public SessionRepository(DatabaseContext ctx) : base(ctx.Sessions)
        {
        }
    }
}
