using API.Mappings;
using API.Models.Entities;
using API.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace API.Infra.Registration
{
    /// <summary>
    /// Register entities into db context
    /// </summary>
    public class DbRegisterContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Migration> Migrations { get; set; }

        public DbRegisterContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Migration>(entity => new MigrationMapping().OnBuild(entity));
            modelBuilder.Entity<User>(entity => new UserMapping().OnBuild(entity));
        }
    }
}
