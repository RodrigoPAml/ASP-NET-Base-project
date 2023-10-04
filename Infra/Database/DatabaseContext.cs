using Infra.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Infra.Database
{
    /// <summary>
    /// Database context base class
    /// </summary>
    public class DatabaseContext : DatabaseRegisterContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            DatabaseInitializer.Init(this);
        }
    }
}