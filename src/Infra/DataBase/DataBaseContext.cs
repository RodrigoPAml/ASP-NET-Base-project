using Microsoft.EntityFrameworkCore;
using API.Infra.Registration;

namespace API.Infra.Database
{
    /// <summary>
    /// Database context base class
    /// </summary>
    public class DataBaseContext : DbRegisterContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
            DataBaseInitializer.Init(this);
        }
    }
}