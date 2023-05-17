using API.Mappings;
using API.Models.Entities;
using API.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace API.Registration
{
    /// <summary>
    /// Register entities into db context
    /// </summary>
    public class DbRegisterContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Migration> Migrations { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Session> Sessions { get; set; }

        public DbRegisterContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Migration>(entity => new MigrationMapping().OnBuild(entity));
            modelBuilder.Entity<User>(entity => new UserMapping().OnBuild(entity));
            modelBuilder.Entity<Movie>(entity => new MovieMapping().OnBuild(entity));
            modelBuilder.Entity<Session>(entity => new SessionMapping().OnBuild(entity));
        }
    }
}
