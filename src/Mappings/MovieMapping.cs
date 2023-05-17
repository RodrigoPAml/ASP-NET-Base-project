using API.Infra.Interfaces;
using API.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Mappings
{
    public class MovieMapping : IEntityMapper<Movie>
    {
        public void OnBuild(EntityTypeBuilder<Movie> entity)
        {
            entity.ToTable("movie", "public");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.Name).HasColumnName("name");
            entity.Property(x => x.Synopsis).HasColumnName("synopsis");
            entity.Property(x => x.Genre).HasColumnName("genre");
            entity.Property(x => x.Duration).HasColumnName("duration");
        }
    }
}
