using API.Infra.Interfaces;
using API.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Mappings
{
    public class SessionMapping : IEntityMapper<Session>
    {
        public void OnBuild(EntityTypeBuilder<Session> entity)
        {
            entity.ToTable("session", "public");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.MovieId).HasColumnName("movie_id");
            entity.Property(x => x.Date).HasColumnName("date");

            entity.HasOne(x => x.Movie)
                .WithMany(x => x.Sessions)
                .HasForeignKey(x => x.MovieId);
        }
    }
}
